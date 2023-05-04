using AutoMapper;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pagination;
using ViewModels.Authentication;
using Repositories.Shared.AuthenticationService;
using Centangle.Common.ResponseHelpers.Models;
using Centangle.Common.ResponseHelpers;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Models.Common;
using ViewModels.Common.Unit;
using ViewModels.Authentication.Approver;
using Helpers.Extensions;
using Repositories.Services.CommonServices.UserService;
using Microsoft.AspNetCore.Identity;
using Models;
using Repositories.Services.CommonServices.ApprovalService.Interface;
using ViewModels.Common.Department;
using Repositories.Shared.UserInfoServices;
using Enums;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq.Dynamic.Core;


namespace Repositories.Services.AppSettingServices.ApproverService
{
    public class ApproverService<CreateViewModel, UpdateViewModel, DetailViewModel> : UserService<CreateViewModel, UpdateViewModel, DetailViewModel>, IApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly IRepositoryResponse _response;

        public ApproverService(ToranceContext db, UserManager<ToranceUser> userManager, ILogger<ApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IIdentityService identity, IRepositoryResponse response, IUserInfoService userInfoService) : base(db, Enums.RolesCatalog.Approver, userManager, logger, mapper, identity, response, userInfoService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
            _response = response;
        }
        protected override async Task<bool> CreateUserAdditionalMappings(CreateViewModel viewModel, SignUpModel model)
        {
            return await SetApproverAssociations((viewModel as ApproverModifyViewModel).Associations, model.Id);
        }
        protected override async Task<bool> UpdateUserAdditionalMappings(UpdateViewModel viewModel, SignUpModel model)
        {
            return await SetApproverAssociations((viewModel as ApproverModifyViewModel).Associations, model.Id);
        }
        public async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await (from u in _db.Users
                                     join ur in _db.UserRoles on u.Id equals ur.UserId
                                     join r in _db.Roles on ur.RoleId equals r.Id
                                     where r.Name == "Approver" && u.Id == id
                                     select u).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var result = _mapper.Map<ApproverDetailViewModel>(dbModel);
                    result.Associations = await GetApproverAssociations(id);
                    result.AccessCode = result.AccessCode != null ? dbModel.AccessCode.DecodeFrom64() : "0";
                    var response = new RepositoryResponseWithModel<ApproverDetailViewModel> { ReturnModel = result };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for Approver");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for Approver threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {

            try
            {
                var searchFilter = search as ApproverSearchViewModel;

                searchFilter.OrderByColumn = string.IsNullOrEmpty(search.OrderByColumn) ? "Id" : search.OrderByColumn;

                var userQueryable = await GetPaginationDbSet(searchFilter);
                searchFilter.OrderByColumn = "";
                var queryString = userQueryable.ToQueryString();

                var users = await userQueryable.Paginate(searchFilter);
                var filteredUserIds = users.Items.Select(x => x.Id);

                var userList = await _db.Users
                    .Where(x => filteredUserIds.Contains(x.Id))
                    .Select(x => new ApproverDetailViewModel
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FullName = x.FullName,
                        UserName = x.UserName,
                        AccessCode = x.AccessCode,
                        CanAddLogs = x.CanAddLogs,
                        ActiveStatus = x.ActiveStatus
                    }).ToListAsync();
                var roles = await _db.UserRoles
                  .Join(_db.Roles.Where(x => x.Name != "SuperAdmin"),
                              ur => ur.RoleId,
                              r => r.Id,
                              (ur, r) => new { UR = ur, R = r })
                  .Where(x => filteredUserIds.Contains(x.UR.UserId))
                  .Select(x => new UserRoleVM
                  {
                      RoleId = x.R.Id,
                      UserId = x.UR.UserId,
                      RoleName = x.R.Name
                  })
                  .ToListAsync();

                //var approverUnits = await _db.ApproverAssociations.Where(x => filteredUserIds.Contains(x.ApproverId)).ToListAsync();

                users.Items.ForEach(x =>
                {
                    x.Id = userList.Where(a => a.Id == x.Id).Select(x => x.Id).FirstOrDefault();
                    x.Email = userList.Where(a => a.Id == x.Id).Select(x => x.Email).FirstOrDefault();
                    x.UserName = userList.Where(a => a.Id == x.Id).Select(x => x.UserName).FirstOrDefault();
                    x.FullName = userList.Where(a => a.Id == x.Id).Select(x => x.FullName).FirstOrDefault();
                    x.AccessCode = userList.Where(a => a.Id == x.Id).Select(x => x.AccessCode).FirstOrDefault();
                    x.CanAddLogs = userList.Where(a => a.Id == x.Id).Select(x => x.CanAddLogs).FirstOrDefault();
                    x.ActiveStatus = userList.Where(a => a.Id == x.Id).Select(x => x.ActiveStatus).FirstOrDefault();
                    x.Roles = roles.Where(u => u.UserId == x.Id).Select(r => new UserRolesVM { Id = r.RoleId, Name = r.RoleName }).ToList();
                    //x.Associations = _mapper.Map<List<UnitBriefViewModel>>(approverUnits.Where(u => u.ApproverId == x.Id).Select(x => x.Unit).ToList());
                });
                var mappedUserList = users.Items as List<M>;
                var paginatedModel = new PaginatedResultModel<M> { Items = mappedUserList, _links = users._links, _meta = users._meta };

                var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<M>>();
                responseModel.ReturnModel = paginatedModel;
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ApproverService GetAll method threw an exception, Message: {ex.Message}");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<IQueryable<ApproverDetailViewModel>> GetPaginationDbSet(ApproverSearchViewModel search)
        {
            var appQueryable = (from user in _db.Users
                                join approverAssociation in _db.ApproverAssociations on user.Id equals approverAssociation.ApproverId into aal
                                from approverAssociation in aal.DefaultIfEmpty()
                                join userRole in _db.UserRoles on user.Id equals userRole.UserId
                                join r in _db.Roles on userRole.RoleId equals r.Id
                                where
                                (
                                   (
                                       string.IsNullOrEmpty(search.Search.value) || user.Email.ToLower().Contains(search.Search.value.ToLower())
                                   )
                                   &&
                                   (search.Unit.Id == null || search.Unit.Id == 0 || approverAssociation.UnitId == search.Unit.Id)
                                   &&
                                   ("Approver" == r.Name)
                                   &&
                                   (
                                       string.IsNullOrEmpty(search.Email) || user.Email.ToLower().Contains(search.Email.ToLower())
                                   )
                               )
                                select user);
            //select new ApproverDetailViewModel { Id = user.Id });
            if (search.IsSearchForm && search.LogType != FilterLogType.None)
            {
                switch (search.LogType)
                {
                    case FilterLogType.Override:
                        appQueryable = JoinApproverWithLogs<OverrideLog>(appQueryable);
                        break;
                    case FilterLogType.TimeOnTools:
                        appQueryable = JoinApproverWithLogs<TOTLog>(appQueryable);
                        break;
                    case FilterLogType.WeldingRodRecord:
                        appQueryable = JoinApproverWithLogs<WRRLog>(appQueryable);
                        break;
                    case FilterLogType.All:
                        return (from ap in appQueryable
                                join tl in _db.TOTLogs on ap.Id equals tl.ApproverId into ttl
                                from tl in ttl.DefaultIfEmpty()
                                join wl in _db.WRRLogs on ap.Id equals wl.ApproverId into wwl
                                from wl in wwl.DefaultIfEmpty()
                                join ol in _db.OverrideLogs on ap.Id equals ol.ApproverId into ool
                                from ol in ool.DefaultIfEmpty()
                                where
                                tl.IsDeleted == false
                                &&
                                wl.IsDeleted == false
                                &&
                                ol.IsDeleted == false
                                group ap by ap.Id
                                        ).Select(x => new ApproverDetailViewModel { Id = x.Key });
                }
            }
            return appQueryable.OrderColumns(search)
                            .Select(x => new ApproverDetailViewModel { Id = x.Id }).GroupBy(x => x.Id)
                            .Select(x => new ApproverDetailViewModel { Id = x.Max(m => m.Id) })
                            .AsQueryable();
        }
        private IQueryable<ToranceUser> JoinApproverWithLogs<T>(IQueryable<ToranceUser> userQueryable, bool isInnerJoin = false) where T : class, IBaseModel, IApproverId
        {
            if (isInnerJoin == false)
                return userQueryable.Join(_db.Set<T>(), l => l.Id, u => u.ApproverId, (u, l) => new { u, l }).Select(x => x.u);
            else
                return userQueryable.GroupJoin(_db.Set<T>(), ol => ol.Id, u => u.ApproverId, (u, ols) => new { u, ols })
                    .SelectMany(x => x.ols.DefaultIfEmpty(), (u, ol) => new { u = u.u, ol = ol })
                    .Where(x => x.ol != null)
                    .Select(x => x.u);
        }
        public async Task<bool> SetApproverAssociations(List<ApproverAssociationsViewModel> associations, long approverId)
        {
            try
            {
                var oldApproverAssociations = await _db.ApproverAssociations.Where(x => x.ApproverId == approverId).ToListAsync();
                if (oldApproverAssociations.Count() > 0)
                {
                    foreach (var oldApproverUnit in oldApproverAssociations)
                    {
                        oldApproverUnit.IsDeleted = true;
                        _db.Entry(oldApproverUnit).State = EntityState.Modified;
                    }
                    await _db.SaveChangesAsync();
                }
                if (associations.Count() > 0)
                {
                    List<ApproverAssociation> list = new List<ApproverAssociation>();
                    foreach (var association in associations)
                    {
                        ApproverAssociation approverUnit = new ApproverAssociation();
                        approverUnit.ApproverId = approverId;
                        approverUnit.DepartmentId = association.Department.Id ?? 0;
                        approverUnit.UnitId = association.Unit.Id ?? 0;
                        list.Add(approverUnit);
                    }
                    await _db.AddRangeAsync(list);
                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ApproverService SetApproverAssociations method threw an exception, Message: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ApproverAssociationsViewModel>> GetApproverAssociations(long id)
        {
            try
            {
                var employeeCrafts = await (from aa in _db.ApproverAssociations
                                            join d in _db.Departments on aa.DepartmentId equals d.Id
                                            join u in _db.Units on aa.UnitId equals u.Id
                                            where aa.ApproverId == id
                                            select new ApproverAssociationsViewModel()
                                            {
                                                Id = aa.Id,
                                                Unit = new UnitBriefViewModel()
                                                {
                                                    Id = u.Id,
                                                    Name = u.Name
                                                },
                                                Department = new DepartmentBriefViewModel()
                                                {
                                                    Id = d.Id,
                                                    Name = d.Name
                                                }
                                            }).ToListAsync();
                return employeeCrafts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ApproverService GetApproverAssociations method threw an exception, Message: {ex.Message}");
                return null;
            }
        }
    }
}
