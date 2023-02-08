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

namespace Repositories.Services.WeldRodRecordServices.EmployeeService
{
    public class EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Employee, CreateViewModel, UpdateViewModel, DetailViewModel>, IEmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly ILogger<EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly IRepositoryResponse _response;

        public EmployeeService(ToranceContext db, UserManager<ToranceUser> userManager, ILogger<EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IIdentityService identity, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
            _response = response;
        }

        public override Expression<Func<Employee, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as EmployeeSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.FirstName.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.FirstName) || x.FirstName.ToLower().Contains(searchFilters.FirstName.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Email) || x.Email.ToLower().Contains(searchFilters.Email.ToLower()))
                        ;
        }
        public override IQueryable<Employee> GetPaginationDbSet()
        {
            return _db.Employees.Include(x => x.Company);
        }

        public override async Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as EmployeeModifyViewModel;
                var user = _mapper.Map<SignUpModel>(model);
                user.AccessCode = viewModel.EmployeeId.EncodePasswordToBase64();
                var userId = await _identity.CreateUser(user, transaction);
                if (userId > 0)
                {
                    var mappedModel = _mapper.Map<Employee>(model);
                    mappedModel.UserId = userId;
                    mappedModel.Contractor = null;
                    mappedModel.Company = null;
                    await _db.AddAsync(mappedModel);
                    await _db.SaveChangesAsync();
                    if (mappedModel.Id > 0)
                    {
                        await transaction.CommitAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                        return response;
                    }
                }
                _logger.LogWarning($"No user for Id:{user.Id} found in Create() Employee");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of Employee ");
                await transaction.RollbackAsync();
                return Response.BadRequestResponse(_response);
            }
        }

        public override async Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as EmployeeModifyViewModel;
                if (model != null)
                {
                    var record = await _db.Employees.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (record != null)
                    {
                        var user = _mapper.Map<SignUpModel>(model);
                        user.AccessCode = viewModel.EmployeeId.EncodePasswordToBase64();
                        var result = await _identity.UpdateUser(user, transaction);
                        if (result)
                        {
                            var dbModel = _mapper.Map(model, record);
                            await _db.SaveChangesAsync();
                            if (dbModel.Id > 0)
                            {
                                await transaction.CommitAsync();
                                var response = new RepositoryResponseWithModel<long> { ReturnModel = dbModel.Id };
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
                await transaction.RollbackAsync();
            }
            return Response.BadRequestResponse(_response);
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.Employees
                    .Include(x => x.Contractor)
                    .Include(x => x.Company)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var result = _mapper.Map<EmployeeDetailViewModel>(dbModel);
                    //result.State = dbModel.State;
                    var response = new RepositoryResponseWithModel<EmployeeDetailViewModel> { ReturnModel = result };
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


        public async Task<bool> IsEmployeeIdUnique(long id, string employeeId)
        {
            return (await _db.Employees.Where(x => x.EmployeeId == employeeId && x.Id != id).CountAsync()) < 1;
        }
        public async Task<bool> IsEmployeeEmailUnique(long id, string email)
        {
            return (await _db.Employees.Where(x => x.Email == email && x.Id != id).CountAsync()) < 1;
        }

        public async Task<IRepositoryResponse> ResetAccessCode(ChangeAccessCodeVM model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentAccessCode, model.EmployeeId);
                if (result.Succeeded)
                {
                    var record = await _db.Employees.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (record != null)
                    {
                        record.EmployeeId = model.EmployeeId;
                        await _db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        var response = new RepositoryResponseWithModel<bool> { ReturnModel = true };
                        return response;
                    }

                }
                await transaction.RollbackAsync();
                return Response.BadRequestResponse(_response);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Response.BadRequestResponse(_response);
            }

        }
    }
}
