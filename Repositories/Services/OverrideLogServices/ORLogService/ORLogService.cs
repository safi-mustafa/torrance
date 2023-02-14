using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Pagination;
using Repositories.Shared;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using Select2.Model;
using System.Linq.Expressions;
using System.Security.Cryptography;
using ViewModels;
using ViewModels.Notification;
using ViewModels.OverrideLogs;
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
        private readonly INotificationService<NotificationModifyViewModel, NotificationModifyViewModel, NotificationModifyViewModel> _notificationService;

        public ORLogService(ToranceContext db, ILogger<ORLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService, INotificationService<NotificationModifyViewModel, NotificationModifyViewModel, NotificationModifyViewModel> notificationService) : base(db, logger, mapper, response, userInfoService, notificationService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
        }

        public override Expression<Func<OverrideLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as ORLogSearchViewModel;
            searchFilters.OrderByColumn = "Status";
            var status = (Status?)((int?)searchFilters.Status);
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var employeeCheck = loggedInUserRole == "Employee";
            var parsedLoggedInId = long.Parse(loggedInUserId);
            searchFilters.StatusNot = loggedInUserRole == "Approver" ? Status.Pending : searchFilters.StatusNot;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Employee.FullName.ToString().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.Requester.Id == null || searchFilters.Requester.Id == 0 || x.Employee.Id == searchFilters.Requester.Id)
                            &&
                            (searchFilters.Approver.Id == null || searchFilters.Approver.Id == 0 || x.Approver.Id == searchFilters.Approver.Id)
                            &&
                            (searchFilters.Unit.Id == null || searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            //&&
                            //(searchFilters.OverrideType == null  || x.OverrideType == searchFilters.OverrideType)
                            &&
                            (searchFilters.Company.Id == null || searchFilters.Company.Id == 0 || x.Company.Id == searchFilters.Company.Id)
                            &&
                            (
                                (loggedInUserRole == "SuperAdmin")
                                ||
                                (loggedInUserRole == "Approver" && x.ApproverId == parsedLoggedInId)
                                ||
                                (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
                            )
                            &&
                            (status == null || status == x.Status)
                            &&
                            (searchFilters.StatusNot == null || searchFilters.StatusNot != x.Status)
                            &&
                            x.IsDeleted == false
            //  &&
            //(!employeeCheck || x.Employees.Any(e => e.EmployeeId.ToString() == loggedInUserId));
            ;
        }

        public async override Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var filters = SetQueryFilter(search);
                var resultQuery = _db.Set<OverrideLog>()
                    .Include(x => x.Unit)
                    //.Include(x => x.Department)
                    .Include(x => x.ReasonForRequest)
                    .Include(x => x.Shift)
                    //.Include(x => x.CraftSkill)
                    //.Include(x => x.CraftRate)
                    .Include(x => x.Employee)
                    .Include(x => x.Company)
                    .Include(x => x.Approver)
                    .Where(filters).IgnoreQueryFilters();
                //var query = resultQuery.ToQueryString();
                var result = await resultQuery.Paginate(search);
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();
                    paginatedResult.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    paginatedResult._meta = result._meta;
                    paginatedResult._links = result._links;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(OverrideLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(OverrideLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.OverrideLogs
                    //.Include(x => x.CraftSkill)
                    //.Include(x => x.CraftRate)
                    .Include(x => x.Contractor)
                    .Include(x => x.ReasonForRequest)
                    //.Include(x => x.OverrideType)
                    .Include(x => x.Department)
                    .Include(x => x.Shift)
                    .Include(x => x.Unit)
                    .Include(x => x.Approver)
                    .Include(x => x.Employee)
                    .Include(x => x.Company)
                    .Where(x => x.Id == id && x.IsDeleted == false).IgnoreQueryFilters().FirstOrDefaultAsync();


                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<ORLogDetailViewModel>(dbModel);
                    mappedModel.Costs = await (from olc in _db.OverrideLogCost
                                               join cs in _db.CraftSkills on olc.CraftSkillId equals cs.Id
                                               where olc.OverrideLogId == dbModel.Id
                                               select new ORLogCostViewModel
                                               {
                                                   Id = olc.Id,
                                                   OverrideHours = olc.OverrideHours,
                                                   CraftSkill = new CraftSkillBriefViewModel()
                                                   {
                                                       Id = cs.Id,
                                                       Name = cs.Name
                                                   },
                                                   OverrideType = olc.OverrideType
                                               }).ToListAsync();
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

        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var costs = model as IORLogCost;
                    var mappedModel = _mapper.Map<OverrideLog>(model);
                    mappedModel.Approver = null;
                    mappedModel.Id = 0;
                    await SetRequesterId(mappedModel);
                    await _db.Set<OverrideLog>().AddAsync(mappedModel);
                    mappedModel.TotalCost = await CalculateTotalCost(costs);
                    mappedModel.TotalHours = CalculateTotalHours(costs);
                    await _db.SaveChangesAsync();
                    await SetORLogCosts(costs, mappedModel.Id);
                    string notificationTitle = "Override Log Created";
                    string notificationMessage = $"A new Override Log with PO# ({mappedModel.PoNumber}) has been created";
                    await _notificationService.Create(new NotificationModifyViewModel(mappedModel.Id, typeof(OverrideLog), mappedModel.ApproverId?.ToString() ?? "", notificationTitle, notificationMessage, NotificationType.Push, NotificationEventTypeCatalog.Created));
                    await transaction.CommitAsync();
                    var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception thrown in Create method of {typeof(OverrideLog).FullName}");
                    await transaction.RollbackAsync();
                    return Response.BadRequestResponse(_response);
                }
            }
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var costs = model as IORLogCost;
                    var updateModel = model as ORLogModifyViewModel;
                    if (updateModel != null)
                    {
                        var record = await _db.Set<OverrideLog>().FindAsync(updateModel?.Id);
                        if (record != null)
                        {
                            if (record.ApproverId != updateModel.Approver.Id)
                            {
                                string notificationTitle = "Override Log Updated";
                                string notificationMessage = $"The Override Log with PO# ({updateModel.PoNumber}) has been updated";
                                await _notificationService.Create(new NotificationModifyViewModel(record.Id, typeof(OverrideLog), updateModel.Approver.Id?.ToString() ?? "", notificationTitle, notificationMessage, NotificationType.Push, NotificationEventTypeCatalog.Updated));
                            }
                            var dbModel = _mapper.Map(model, record);
                            dbModel.Approver = null;
                            dbModel.TotalCost = await CalculateTotalCost(costs);
                            dbModel.TotalHours = CalculateTotalHours(costs);
                            await SetRequesterId(dbModel);
                            await _db.SaveChangesAsync();
                            await SetORLogCosts(costs, dbModel.Id);
                            await transaction.CommitAsync();
                            var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                            return response;
                        }
                        _logger.LogWarning($"Record for id: {updateModel?.Id} not found in ORLogService in Update()");
                    }
                    return Response.NotFoundResponse(_response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Update() for ORLogService threw the following exception");
                    await transaction.RollbackAsync();
                    return Response.BadRequestResponse(_response);
                }
            }
        }

        public async Task<IRepositoryResponse> GetOverrideTypes<BaseBriefVM>(IBaseSearchModel search)
        {
            try
            {

                List<Select2ViewModel> list = new List<Select2ViewModel>();
                foreach (OverrideTypeCatalog overrideType in (OverrideTypeCatalog[])Enum.GetValues(typeof(OverrideTypeCatalog)))
                {
                    list.Add(new Select2ViewModel()
                    {
                        id = ((int)overrideType).ToString(),
                        text = overrideType.ToString()
                    });
                }

                if (list != null && list.Count > 0)
                {
                    var paginatedResult = new PaginatedResultModel<Select2ViewModel>();
                    paginatedResult.Items = list;
                    paginatedResult._meta = new();
                    paginatedResult._links = new();
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<Select2ViewModel>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(TOTLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(TOTLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task SetRequesterId(OverrideLog mappedModel)
        {
            var role = _userInfoService.LoggedInUserRole();
            if (role == "Employee")
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInUserId());
                mappedModel.CompanyId = (await _db.Users.Where(x => x.Id == mappedModel.EmployeeId).Select(x => x.CompanyId).FirstOrDefaultAsync()) ?? 0;
            }
            else if (mappedModel.EmployeeId == null || mappedModel.EmployeeId < 1)
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInUserId());
            }

        }
        public async Task<bool> SetORLogCosts(IORLogCost overrideLogCost, long id)
        {
            try
            {
                var oldCosts = await _db.OverrideLogCost.Where(x => x.OverrideLogId == id).ToListAsync();
                _db.OverrideLogCost.RemoveRange(oldCosts);
                if (overrideLogCost.Costs.Count() > 0)
                {
                    List<OverrideLogCost> list = new List<OverrideLogCost>();
                    foreach (var cost in overrideLogCost.Costs)
                    {
                        OverrideLogCost dbCost = new OverrideLogCost();
                        dbCost.OverrideHours = cost.OverrideHours;
                        dbCost.CraftSkillId = cost.CraftSkill.Id ?? 0;
                        dbCost.OverrideType = cost.OverrideType;
                        dbCost.OverrideLogId = id;
                        list.Add(dbCost);
                    }
                    await _db.AddRangeAsync(list);
                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError($"ORLogService SetORLogCosts method threw an exception, Message: {ex.Message}");
                throw ex;
            }
        }

        private async Task<double> CalculateTotalCost(IORLogCost overrideLogCost)
        {
            if (overrideLogCost.Costs == null || overrideLogCost.Costs.Count < 1)
            {
                return 0;
            }
            var crafts = await _db.CraftSkills.Where(x => x.IsDeleted == false).ToListAsync();
            double totalCost = 0;
            foreach (var cost in overrideLogCost.Costs)
            {
                double craftCost = 0;
                var selectedCraft = crafts.Where(x => x.Id == cost.CraftSkill.Id).FirstOrDefault();
                if (selectedCraft != null)
                {
                    if (cost.OverrideType == OverrideTypeCatalog.ST)
                    {
                        craftCost = selectedCraft.STRate;
                    }
                    else if (cost.OverrideType == OverrideTypeCatalog.OT)
                    {
                        craftCost = selectedCraft.OTRate;
                    }
                    else
                    {
                        craftCost = selectedCraft.DTRate;
                    }
                }

                totalCost += cost.OverrideHours * craftCost;
            }
            return totalCost;

        }

        private double CalculateTotalHours(IORLogCost overrideLogCost)
        {
            if (overrideLogCost.Costs == null || overrideLogCost.Costs.Count < 1)
            {
                return 0;
            }
            return overrideLogCost.Costs.Sum(x => x.OverrideHours);

        }
    }

}
