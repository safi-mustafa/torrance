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
using ViewModels.Authentication.Approver;

namespace Repositories.Services.WeldRodRecordServices.ApproverService
{
    public class ApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>: IApproverService<CreateViewModel, UpdateViewModel, DetailViewModel>
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
                var user = _mapper.Map<SignUpModel>(model);
                user.Role = "Approver";
                var userId = await _identity.CreateUser(user, transaction);
                if (userId > 0)
                {
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
                var dbModel = await _db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var result = _mapper.Map<UserDetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<UserDetailViewModel> { ReturnModel = result };
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
            var searchFilter = search as UserSearchViewModel;
            searchFilter.Roles = new List<UserRolesVM>
            {
                new UserRolesVM()
                {
                    Id = 0,
                    Name = "Approver"
                }
            };
            var result = await _identity.GetAll<M>(searchFilter);
            var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<M>>();
            responseModel.ReturnModel = result;
            return responseModel;
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
    }
}
