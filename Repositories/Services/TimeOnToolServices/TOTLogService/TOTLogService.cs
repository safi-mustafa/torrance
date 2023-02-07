using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using Repositories.Shared;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using Select2.Model;
using System.Data;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using ViewModels;
using ViewModels.Notification;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public class TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<TOTLog, CreateViewModel, UpdateViewModel, DetailViewModel>, ITOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService _notificationService;

        public TOTLogService(ToranceContext db, ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService, INotificationService notificationService) : base(db, logger, mapper, response, notificationService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
        }

        public override Expression<Func<TOTLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as TOTLogSearchViewModel;
            //searchFilters.OrderByColumn = "Status";
            var status = (Status?)((int?)searchFilters.Status);
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.EquipmentNo.ToString().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.EquipmentNo == null || x.EquipmentNo == searchFilters.EquipmentNo)
                            &&
                            (searchFilters.Shift.Id == null || x.Shift.Id == searchFilters.Shift.Id)
                            &&
                            (searchFilters.DelayType.Id == null || x.DelayType.Id == searchFilters.DelayType.Id)
                            &&
                            (searchFilters.Requester.Id == null || x.Employee.Id == searchFilters.Requester.Id)
                            &&
                            (searchFilters.Approver.Id == null || x.Approver.Id == searchFilters.Approver.Id)
                            &&
                            (searchFilters.PermitType.Id == null || x.PermitType.Id == searchFilters.PermitType.Id)
                            &&
                            (searchFilters.Unit.Id == 0 || searchFilters.Unit.Id == null || x.Unit.Id == searchFilters.Unit.Id)
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
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.TOTLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Company)
                    .Include(x => x.ReworkDelay)
                    .Include(x => x.ShiftDelay)
                    .Include(x => x.Shift)
                    .Include(x => x.PermitType)
                    .Include(x => x.Approver)
                    .Include(x => x.Foreman)
                    .Include(x => x.Employee)
                    .Include(x => x.PermittingIssue)
                    .Include(x => x.DelayType)
                    .Include(x => x.ReasonForRequest)
                    .Where(x => x.Id == id && x.IsDeleted == false).IgnoreQueryFilters().FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<TOTLogDetailViewModel>(dbModel);
                    mappedModel.TWRModel = new TWRViewModel(mappedModel.Twr);
                    var response = new RepositoryResponseWithModel<TOTLogDetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for TOTLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for TOTLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public override async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var filters = SetQueryFilter(search);
                var resultQuery = _db.Set<TOTLog>()
                    .Include(x => x.Unit)
                    .Include(x => x.DelayType)
                    .Include(x => x.Shift)
                    .Include(x => x.PermitType)
                    .Include(x => x.Employee)
                    .Include(x => x.Company)
                    .Include(x => x.ReasonForRequest)
                    .Where(filters).IgnoreQueryFilters();
                var query = resultQuery.ToQueryString();
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
                _logger.LogWarning($"No record found for {typeof(TOTLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(TOTLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var mappedModel = _mapper.Map<TOTLog>(model);
                    await SetRequesterId(mappedModel);
                    await _db.Set<TOTLog>().AddAsync(mappedModel);
                    var result = await _db.SaveChangesAsync() > 0;
                    await _notificationService.AddNotificationAsync(new NotificationViewModel(mappedModel.Id, typeof(OverrideLog), mappedModel.ApproverId?.ToString() ?? "", "TOT Log Created", "A new TOT Log has been created", NotificationType.Push, NotificationEntityType.Logs));
                    await transaction.CommitAsync();
                    var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                    return response;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, $"Exception thrown in Create method of {typeof(TOTLog).FullName}");
                    return Response.BadRequestResponse(_response);
                }
            }
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            try
            {
                var updateModel = model as TOTLogModifyViewModel;
                if (updateModel != null)
                {
                    var record = await _db.Set<TOTLog>().FindAsync(updateModel?.Id);
                    if (record != null)
                    {
                        if (record.ApproverId != updateModel.Approver.Id)
                        {
                            await _notificationService.AddNotificationAsync(new NotificationViewModel(record.Id, typeof(TOTLog), updateModel.Approver.Id?.ToString() ?? "", "TOT Log updated", "You have a new log to approve.", NotificationType.Push, NotificationEntityType.Logs));
                        }
                        var dbModel = _mapper.Map(model, record);
                        await SetRequesterId(dbModel);
                        await _db.SaveChangesAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                        return response;
                    }
                    _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(TOTLog).FullName} in Update()");
                }
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for {typeof(TOTLog).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task SetRequesterId(TOTLog mappedModel)
        {
            var role = _userInfoService.LoggedInUserRole();
            if (role == "Employee")
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInEmployeeId());
            }
            mappedModel.CompanyId = await _db.Employees.Where(x => x.Id == mappedModel.EmployeeId).Select(x => x.CompanyId).FirstOrDefaultAsync();
        }

        public async Task<IRepositoryResponse> GetTWRNumericValues<BaseBriefVM>(IBaseSearchModel search)
        {
            try
            {
                var list = GetTWRNumericList();


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

        public async Task<IRepositoryResponse> GetTWRAphabeticValues<M>(IBaseSearchModel search)
        {
            try
            {
                var list = GetTWRAlphabeticList();



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

        public static List<Select2ViewModel> GetTWRNumericList() => new TWRViewModel().GetTWRNumericList();

        public static List<Select2ViewModel> GetTWRAlphabeticList() => new TWRViewModel().GetTWRAlphabeticList();

    }
}
