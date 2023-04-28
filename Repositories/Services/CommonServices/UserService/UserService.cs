using AutoMapper;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Authentication;
using ViewModels.WeldingRodRecord.Employee;
using Repositories.Shared.AuthenticationService;
using Centangle.Common.ResponseHelpers.Models;
using Centangle.Common.ResponseHelpers;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Microsoft.AspNetCore.Identity;
using Models;
using Helpers.Extensions;
using ViewModels.Authentication.Approver;
using ViewModels.Authentication.User;
using Enums;
using Repositories.Shared.UserInfoServices;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Repositories.Services.CommonServices.UserService
{
    public class UserService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<ToranceUser, CreateViewModel, UpdateViewModel, DetailViewModel>, IUserService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly RolesCatalog _role;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;

        public UserService
            (
                ToranceContext db, 
                RolesCatalog role, 
                UserManager<ToranceUser> userManager, 
                ILogger logger, 
                IMapper mapper, 
                IIdentityService identity, 
                IRepositoryResponse response, 
                IUserInfoService userInfoService) : base(db, logger, mapper, response)
        {
            _db = db;
            _role = role;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
            _response = response;
            _userInfoService = userInfoService;
        }

        public override async Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as UserUpdateViewModel;
                var user = _mapper.Map<SignUpModel>(model);
                user.AccessCode = viewModel.AccessCode.EncodePasswordToBase64();
                user.Role = _role.ToString();
                user.Id = await _identity.CreateUser(user);
                if (user.Id > 0)
                {
                    var result = await CreateUserAdditionalMappings(model, user);
                    if (result)
                    {
                        await transaction.CommitAsync();
                        //var response = new RepositoryResponseWithModel<UserCreateResponseViewModel>
                        //{
                        //    ReturnModel = new UserCreateResponseViewModel
                        //    {
                        //        Id = user.Id,
                        //        Message = "Account created successfully."// Please login using the pin used at the time of signup.
                        //    }
                        //};
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = user.Id };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of User Creation ");
            }
            await transaction.RollbackAsync();
            return Response.BadRequestResponse(_response);
        }

        public override async Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as UserUpdateViewModel;
                if (model != null)
                {
                    var record = await _db.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (record != null)
                    {
                        var user = _mapper.Map<SignUpModel>(model);
                        user.Role = _role.ToString();
                        user.AccessCode = record.AccessCode.DecodeFrom64();
                        var result = await _identity.UpdateUser(user);
                        if (result)
                        {
                            var mappingsModified = await UpdateUserAdditionalMappings(model, user);
                            if (mappingsModified)
                            {
                                await transaction.CommitAsync();
                                var response = new RepositoryResponseWithModel<long> { ReturnModel = user.Id };
                                return response;
                            }
                        }
                    }
                    _logger.LogWarning($"Record for id: {model?.Id} not found in Employee");
                    await transaction.RollbackAsync();
                    return Response.NotFoundResponse(_response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for Employee threw the following exception");
            }
            await transaction.RollbackAsync();
            return Response.BadRequestResponse(_response);
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await GetUserQueryable()
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var result = _mapper.Map<DetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<DetailViewModel> { ReturnModel = result };
                    return response;
                    //return await base.GetById(id);
                }
                _logger.LogWarning($"No record found for id:{id} for Employee");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for Employee threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }


        public async Task<bool> IsAccessCodeUnique(long id, string accessCode)
        {
            var encodedAccessCode = accessCode.EncodePasswordToBase64();
            if (accessCode == "9999")
                return false;
            bool isUnique = (await _db.Users.Where(x => x.AccessCode == encodedAccessCode && x.Id != id && x.IsDeleted == false).CountAsync()) < 1;
            return isUnique;
        }
        public async Task<bool> IsEmailUnique(long id, string email)
        {
            return (await _db.Users.Where(x => x.Email == email && x.Id != id && x.IsDeleted == false).CountAsync()) < 1;
        }

        public async Task<IRepositoryResponse> ResetAccessCode(ChangeAccessCodeVM model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                user.AccessCode = model.AccessCode.EncodePasswordToBase64();
                await _db.SaveChangesAsync();
                var response = new RepositoryResponseWithModel<bool> { ReturnModel = true };
                return response;
            }
            catch (Exception ex)
            {
                return Response.BadRequestResponse(_response);
            }

        }

        protected virtual IQueryable<ToranceUser> GetUserQueryable()
        {
            return _db.Users.Include(x => x.Company);
        }

        public override Expression<Func<ToranceUser, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as UserSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.FullName.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.FullName) || x.FullName.ToLower().Contains(searchFilters.FullName.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Email) || x.Email.ToLower().Contains(searchFilters.Email.ToLower()));
        }
        public override IQueryable<ToranceUser> GetPaginationDbSet()
        {
            return (from user in _db.Users.Include(x => x.Company)
                    join userRole in _db.UserRoles on user.Id equals userRole.UserId
                    join r in _db.Roles on userRole.RoleId equals r.Id
                    where r.Name == _role.ToString()
                    select user
                    );
        }
        protected virtual async Task<bool> CreateUserAdditionalMappings(CreateViewModel viewModel, SignUpModel model)
        {
            return true;
        }
        protected virtual async Task<bool> UpdateUserAdditionalMappings(UpdateViewModel viewModel, SignUpModel model)
        {
            return true;
        }

        public async Task<bool> ValidatePassword(string password)
        {
            try
            {
                var loggedInUserId = long.Parse(_userInfoService.LoggedInUserId());
                var user = await _db.Users.Where(x => x.Id == loggedInUserId).FirstOrDefaultAsync();
                var result = await _userManager.CheckPasswordAsync(user, password);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
