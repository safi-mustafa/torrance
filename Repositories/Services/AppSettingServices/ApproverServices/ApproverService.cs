using AutoMapper;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Authentication;
using Repositories.Shared.AuthenticationService;
using Centangle.Common.ResponseHelpers.Models;
using Centangle.Common.ResponseHelpers;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Models;
using ViewModels;
using Models.Common;
using ViewModels.Common.Unit;
using ViewModels.Authentication.Approver;
using Helpers.Extensions;
using System.Net;

namespace Repositories.Services.WeldRodRecordServices.ApproverService
{
    public class ApproverService<CreateViewModel, UpdateViewModel, DetailViewModel> : IApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly IRepositoryResponse _response;

        public ApproverService(ToranceContext db, ILogger<ApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IIdentityService identity, IRepositoryResponse response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
            _response = response;
        }

        public async Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as ApproverModifyViewModel;
                var user = _mapper.Map<SignUpModel>(model);
                user.Role = "Approver";
                user.AccessCode = user.AccessCode.EncodePasswordToBase64();
                var userId = await _identity.CreateUser(user, transaction);
                if (userId > 0)
                {
                    await SetApproverUnits(viewModel.UnitIds, userId);
                    await transaction.CommitAsync();
                    var response = new RepositoryResponseWithModel<long> { ReturnModel = userId };
                    return response;
                }
                _logger.LogWarning($"No user for Id:{user.Id} found in Create() Approver");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of Approver ");
                await transaction.RollbackAsync();
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<IRepositoryResponse> Update(UpdateViewModel model)
        {

            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as ApproverModifyViewModel;
                if (model != null)
                {
                    var record = await _db.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (record != null)
                    {
                        var user = _mapper.Map<SignUpModel>(model);
                        user.Role = "Approver";
                        var result = await _identity.UpdateUser(user, transaction);
                        if (result)
                        {
                            await SetApproverUnits(viewModel.UnitIds, model.Id);
                            await transaction.CommitAsync();
                            var response = new RepositoryResponseWithModel<long> { ReturnModel = user.Id };
                            return response;
                        }
                    }
                    _logger.LogWarning($"Record for id: {model?.Id} not found in Approver");
                    await transaction.RollbackAsync();
                    return Response.NotFoundResponse(_response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for Approver threw the following exception");
                await transaction.RollbackAsync();
            }
            return Response.BadRequestResponse(_response);
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
                    result.Units = await GetApproverUnits(id);
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

        public async Task<bool> IsApproverEmailUnique(long id, string email)
        {
            return (await _db.Users.Where(x => x.Email == email && x.Id != id).CountAsync()) < 1;
        }


        public async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {

            try
            {
                var searchFilter = search as ApproverSearchViewModel;

                searchFilter.OrderByColumn = string.IsNullOrEmpty(search.OrderByColumn) ? "Id" : search.OrderByColumn;

                var userQueryable = (from user in _db.Users
                                     join approverUnit in _db.ApproverUnits on user.Id equals approverUnit.ApproverId into aul
                                     from approverUnit in aul.DefaultIfEmpty()
                                     join userRole in _db.UserRoles on user.Id equals userRole.UserId
                                     join r in _db.Roles on userRole.RoleId equals r.Id
                                     where
                                     (
                                        (
                                            string.IsNullOrEmpty(searchFilter.Search.value) || user.Email.ToLower().Contains(searchFilter.Search.value.ToLower())
                                        )
                                        &&
                                        (searchFilter.Unit.Id == null || searchFilter.Unit.Id == 0 || approverUnit.UnitId == searchFilter.Unit.Id)
                                        &&
                                        ("Approver" == r.Name)
                                        &&
                                        (
                                            string.IsNullOrEmpty(searchFilter.Email) || user.Email.ToLower().Contains(searchFilter.Email.ToLower())
                                        )
                                    )
                                     select new ApproverDetailViewModel { Id = user.Id }
                            ).GroupBy(x => x.Id)
                            .Select(x => new ApproverDetailViewModel { Id = x.Max(m => m.Id) })
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
                        UserName = x.UserName,
                        PhoneNumber = x.PhoneNumber,
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

                var approverUnits = await _db.ApproverUnits.Include(x => x.Unit).Where(x => filteredUserIds.Contains(x.ApproverId)).ToListAsync();

                users.Items.ForEach(x =>
                {
                    x.Id = userList.Where(a => a.Id == x.Id).Select(x => x.Id).FirstOrDefault();
                    x.Email = userList.Where(a => a.Id == x.Id).Select(x => x.Email).FirstOrDefault();
                    x.PhoneNumber = userList.Where(a => a.Id == x.Id).Select(x => x.PhoneNumber).FirstOrDefault();
                    x.UserName = userList.Where(a => a.Id == x.Id).Select(x => x.UserName).FirstOrDefault();
                    x.Roles = roles.Where(u => u.UserId == x.Id).Select(r => new UserRolesVM { Id = r.RoleId, Name = r.RoleName }).ToList();
                    x.Units = _mapper.Map<List<UnitBriefViewModel>>(approverUnits.Where(u => u.ApproverId == x.Id).Select(x => x.Unit).ToList());
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

        public async Task<IRepositoryResponse> Delete(long id)
        {
            try
            {
                var dbModel = await _db.Users.FindAsync(id);
                if (dbModel != null)
                {
                    dbModel.IsDeleted = true;
                    await _db.SaveChangesAsync();
                    return _response;
                }
                _logger.LogWarning($"No record found for id:{id} for Approver in Delete()");

                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete() for Approver threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<bool> SetApproverUnits(List<long> unitIds, long approverId)
        {
            try
            {
                var oldApproverUnits = await _db.ApproverUnits.Where(x => x.ApproverId == approverId).ToListAsync();
                if (oldApproverUnits.Count() > 0)
                {
                    foreach (var oldApproverUnit in oldApproverUnits)
                    {
                        oldApproverUnit.IsDeleted = true;
                        _db.Entry(oldApproverUnit).State = EntityState.Modified;
                    }
                    _db.SaveChanges();
                }
                if (unitIds.Count() > 0)
                {
                    List<ApproverUnit> list = new List<ApproverUnit>();
                    foreach (var unitId in unitIds)
                    {
                        ApproverUnit approverUnit = new ApproverUnit();
                        approverUnit.ApproverId = approverId;
                        approverUnit.UnitId = unitId;
                        list.Add(approverUnit);
                    }
                    await _db.AddRangeAsync(list);
                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ApproverService SetApproverUnits method threw an exception, Message: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UnitBriefViewModel>> GetApproverUnits(long id)
        {
            try
            {
                var employeeCrafts = await (from au in _db.ApproverUnits
                                            where au.ApproverId == id
                                            join u in _db.Units on au.UnitId equals u.Id
                                            select new UnitBriefViewModel()
                                            {
                                                Id = au.UnitId,
                                                Name = u.Name
                                            }).ToListAsync();
                return employeeCrafts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ApproverService GetApproverUnits method threw an exception, Message: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsAccessCodeUnique(long id, string accessCode)
        {
            var encodedAccessCode = accessCode.EncodePasswordToBase64();
            return (await _db.Users.Where(x => x.AccessCode == encodedAccessCode && x.Id != id).CountAsync()) < 1;
        }
    }
}
