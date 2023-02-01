using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Pagination;
using Repositories.Shared;
using Repositories.Shared.UserInfoServices;
using System.Linq.Expressions;
using ViewModels;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord;
using ViewModels.WeldingRodRecord.Employee;

namespace Repositories.Services.OverrideLogServices.ORLogService
{
    public class ORLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<OverrideLog, CreateViewModel, UpdateViewModel, DetailViewModel>, IORLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ORLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;

        public ORLogService(ToranceContext db, ILogger<ORLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
        }

        public override Expression<Func<OverrideLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as ORLogSearchViewModel;
            searchFilters.OrderByColumn = "Status";
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.RequesterEmail.ToString().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.RequesterEmail) || x.RequesterEmail == searchFilters.RequesterEmail)
                            &&
                            (searchFilters.Contractor.Id == 0 || x.Contractor.Id == searchFilters.Contractor.Id)
            //&&
            //(
            //    (loggedInUserRole == "SuperAdmin" || loggedInUserRole == "Admin")
            //    ||
            //    (loggedInUserRole == "Approver" && x.ApproverId == parsedLoggedInId)
            //    ||
            //    (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
            //)
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.OverrideLogs
                    .Include(x => x.CraftSkill)
                    .Include(x => x.CraftRate)
                    .Include(x => x.Contractor)
                    .Include(x => x.ReasonForRequest)
                    .Include(x => x.OverrideType)
                    .Include(x => x.Shift)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<ORLogDetailViewModel>(dbModel);
                    mappedModel.Employees = await GetOverrideLogEmployees(id);
                    var response = new RepositoryResponseWithModel<ORLogDetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for ORLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for ORLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<bool> SetOverrideLogEmployees(List<long> employeeIds, long overrideLogId)
        {
            try
            {
                var oldOREmployees = await _db.OverrideLogEmployees.Where(x => x.OverrideLogId == overrideLogId).ToListAsync();
                if (oldOREmployees.Count() > 0)
                {
                    foreach (var oldOREmployee in oldOREmployees)
                    {
                        oldOREmployee.IsDeleted = true;
                        _db.Entry(oldOREmployee).State = EntityState.Modified;
                    }
                    _db.SaveChanges();
                }
                if (employeeIds.Count() > 0)
                {
                    List<OverrideLogEmployee> list = new List<OverrideLogEmployee>();
                    foreach (var employeeId in employeeIds)
                    {
                        OverrideLogEmployee orLogEmployee = new OverrideLogEmployee();
                        orLogEmployee.OverrideLogId = overrideLogId;
                        orLogEmployee.EmployeeId = employeeId;
                        list.Add(orLogEmployee);
                    }
                    await _db.AddRangeAsync(list);
                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"OverrideLogService SetOverrideLogEmployees method threw an exception, Message: {ex.Message}");
                return false;
            }
        }

        public async Task<List<EmployeeBriefViewModel>> GetOverrideLogEmployees(long id)
        {
            try
            {
                var ORLogEmployees = await (from oe in _db.OverrideLogEmployees
                                            where oe.OverrideLogId == id
                                            join c in _db.Employees on oe.EmployeeId equals c.Id
                                            select new EmployeeBriefViewModel()
                                            {
                                                Id = oe.EmployeeId,
                                                Name = c.FirstName + " " + c.LastName,
                                            }).ToListAsync();
                return ORLogEmployees;
            }
            catch (Exception ex)
            {
                _logger.LogError($"OverrideLogService GetOverrideLogEmployees method threw an exception, Message: {ex.Message}");
                return null;
            }
        }

    }
}
