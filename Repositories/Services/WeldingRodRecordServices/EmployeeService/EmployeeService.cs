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

namespace Repositories.Services.WeldRodRecordServices.EmployeeService
{
    public class EmployeeService : BaseService<Employee, EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel>, IEmployeeService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;

        public EmployeeService(ToranceContext db, ILogger<EmployeeService> logger, IMapper mapper, IIdentityService identity) : base(db, logger, mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
        }

        public override Expression<Func<Employee, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as EmployeeSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.FirstName.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.FirstName) || x.FirstName.ToLower().Contains(searchFilters.FirstName.ToLower()))
                            &&
                            (searchFilters.Status == null || x.ActiveStatus == searchFilters.Status)
                        ;
        }

        public override async Task<long> Create(EmployeeModifyViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var user = _mapper.Map<SignUpModel>(model);
                var userId = await _identity.CreateUser(user, transaction);
                if (userId > 0)
                {
                    var mappedModel = _mapper.Map<Employee>(model);
                    mappedModel.UserId = userId;
                    await _db.AddAsync(mappedModel);
                    await _db.SaveChangesAsync();
                    if (mappedModel.Id > 0)
                    {
                        await transaction.CommitAsync();
                        return mappedModel.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of Employee ");
                await transaction.RollbackAsync();
            }
            return -1;
        }

        public override async Task<long> Update(EmployeeModifyViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                if (model != null)
                {
                    var record = await _db.Employees.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (record != null)
                    {
                        var user = _mapper.Map<SignUpModel>(model);
                        var response = await _identity.UpdateUser(user, transaction);
                        if (response)
                        {
                            var dbModel = _mapper.Map(model, record);
                            await _db.SaveChangesAsync();
                            if (dbModel.Id > 0)
                            {
                                await transaction.CommitAsync();
                                return dbModel.Id;
                            }
                        }
                    }
                    _logger.LogWarning($"Record for id: {model?.Id} not found in Employee");
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for Employee threw the following exception");
            }
            await transaction.RollbackAsync();
            return -1;
        }

        public override async Task<EmployeeDetailViewModel> GetById(long id)
        {
            try
            {
                var dbModel = await _db.Employees.Include(x => x.Approver).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var result = _mapper.Map<EmployeeDetailViewModel>(dbModel);
                    result.State = dbModel.State;
                    return result;
                    //return await base.GetById(id);
                }
                _logger.LogWarning($"No record found for id:{id} for Employee");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for Employee threw the following exception");
            }
            return new();
        }

      
        public async Task<bool> IsEmployeeIdUnique(long id, string employeeId)
        {
            return (await _db.Employees.Where(x => x.EmployeeId == employeeId && x.Id != id).CountAsync()) < 1;
        }
        public async Task<bool> IsEmployeeEmailUnique(long id, string email)
        {
            return (await _db.Employees.Where(x => x.Email == email && x.Id != id).CountAsync()) < 1;
        }
    }
}
