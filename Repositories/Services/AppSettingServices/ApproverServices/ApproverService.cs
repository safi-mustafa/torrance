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

                var userQueryable = (from user in _db.Users
                                     join approverAssociation in _db.ApproverAssociations on user.Id equals approverAssociation.ApproverId into aal
                                     from approverAssociation in aal.DefaultIfEmpty()
                                     join userRole in _db.UserRoles on user.Id equals userRole.UserId
                                     join r in _db.Roles on userRole.RoleId equals r.Id
                                     where
                                      (
                                         (
                                             string.IsNullOrEmpty(searchFilter.Search.value) || user.Email.ToLower().Contains(searchFilter.Search.value.ToLower())
                                         )
                                         &&
                                         (searchFilter.Unit.Id == null || searchFilter.Unit.Id == 0 || approverAssociation.UnitId == searchFilter.Unit.Id)
                                         &&
                                         ("Approver" == r.Name)
                                         &&
                                         (
                                             string.IsNullOrEmpty(searchFilter.Email) || user.Email.ToLower().Contains(searchFilter.Email.ToLower())
                                         )
                                     )
                                     select new ApproverDetailViewModel { Id = user.Id, Email = user.Email, FullName = user.FullName }
                            ).GroupBy(x => x.Id)
                            .Select(x => new ApproverDetailViewModel
                            {
                                Id = x.Max(m => m.Id),
                                Email = x.Max(m => m.Email),
                                FullName = x.Max(m => m.FullName),

                            })
                            .AsQueryable();

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
                    _db.SaveChanges();
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
