using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using ClosedXML.Excel;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Services.CommonServices.PossibleApproverService;
using Repositories.Shared;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using Select2.Model;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using ViewModels.Notification;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.Shared.Interfaces;
using ViewModels.TimeOnTools.TOTLog;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public class TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<TOTLog, CreateViewModel, UpdateViewModel, DetailViewModel>, ITOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IDelayType, ITOTLogNotificationViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, IDelayType, ITOTLogNotificationViewModel, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPossibleApproverService _possibleApproverService;

        public TOTLogService(ToranceContext db, ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService, INotificationService notificationService, IHttpContextAccessor httpContextAccessor, IPossibleApproverService possibleApproverService) : base(db, logger, mapper, response, userInfoService, notificationService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _possibleApproverService = possibleApproverService;
        }

        public override Expression<Func<TOTLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as TOTLogSearchViewModel;
            var status = (Status?)((int?)searchFilters.Status);
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == RolesCatalog.Employee.ToString() || loggedInUserRole == RolesCatalog.CompanyManager.ToString() || searchFilters.IsExcelDownload)
            {
                searchFilters.StatusNot = null;
            }
            else
            {
                searchFilters.StatusNot = Status.Pending;
            }

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Employee.FullName.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.EquipmentNo) || x.EquipmentNo.ToLower().Contains(searchFilters.EquipmentNo.ToLower()))
                            &&
                            (searchFilters.Shift.Id == null || searchFilters.Shift.Id == 0 || x.Shift.Id == searchFilters.Shift.Id)
                            &&
                            (searchFilters.DelayType.Id == null || searchFilters.DelayType.Id == 0 || x.DelayType.Id == searchFilters.DelayType.Id)
                            &&
                            (searchFilters.Requestor.Id == null || searchFilters.Requestor.Id == 0 || x.Employee.Id == searchFilters.Requestor.Id)
                            &&
                            (searchFilters.Approver.Id == null || searchFilters.Approver.Id == 0 || x.Approver.Id == searchFilters.Approver.Id)
                            &&
                            (searchFilters.PermitType.Id == null || searchFilters.PermitType.Id == 0 || x.PermitType.Id == searchFilters.PermitType.Id)
                            &&
                            (searchFilters.Unit.Id == null || searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            &&
                            (searchFilters.Department.Id == null || searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Company.Id == null || searchFilters.Company.Id == 0 || x.Company.Id == searchFilters.Company.Id)
                            &&
                            (
                                (loggedInUserRole == "SuperAdmin")
                                ||
                                 (loggedInUserRole == RolesCatalog.Administrator.ToString())
                                ||

                                (loggedInUserRole == "Approver" && x.ApproverId == parsedLoggedInId)
                                ||
                                (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
                            )
                            &&
                            (searchFilters.SelectedIds == null || searchFilters.SelectedIds.Count <= 0 || searchFilters.SelectedIds.Contains(x.Id.ToString()) || x.Status == Status.Pending)
                            &&
                            (status == null || status == x.Status)
                            &&
                            (searchFilters.StatusNot == null || searchFilters.StatusNot != x.Status)
                            &&
                            x.IsDeleted == false
                            &&
                            x.IsArchived == searchFilters.IsArchived
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var queryable = _db.TOTLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Company)
                    .Include(x => x.ReworkDelay)
                    .Include(x => x.ShiftDelay)
                    .Include(x => x.StartOfWorkDelay)
                    .Include(x => x.OngoingWorkDelay)
                    .Include(x => x.Shift)
                    .Include(x => x.PermitType)
                    .Include(x => x.Approver)
                    //.Include(x => x.Foreman)
                    .Include(x => x.Employee)
                    .Include(x => x.PermittingIssue)
                    .Include(x => x.DelayType)
                    .Include(x => x.ReasonForRequest)
                    .Where(x => x.Id == id && x.IsDeleted == false).IgnoreQueryFilters();
                var dbModel = await queryable.FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<TOTLogDetailViewModel>(dbModel);
                    mappedModel.PossibleApprovers = await _possibleApproverService.GetPossibleApprovers(mappedModel.Unit.Id, mappedModel.Department.Id);
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
                    .Include(x => x.ReworkDelay)
                    .Include(x => x.ShiftDelay)
                    .Include(x => x.StartOfWorkDelay)
                    .Include(x => x.OngoingWorkDelay)
                    .Include(x => x.Shift)
                    .Include(x => x.Department)
                    .Include(x => x.PermitType)
                    .Include(x => x.Employee)
                    .Include(x => x.Company)
                    .Include(x => x.ReasonForRequest)
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
                    SetDelayReasonFields(mappedModel, model);
                    await _db.Set<TOTLog>().AddAsync(mappedModel);
                    var result = await _db.SaveChangesAsync() > 0;
                    await _notificationService.CreateNotificationsForLogCreation(new TOTLogNotificationViewModel(model, mappedModel));
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
                        var previousApproverId = record.ApproverId;
                        var previousStatus = record.Status;
                        var dbModel = _mapper.Map(model, record);
                        dbModel.Status = previousStatus;
                        if (previousApproverId != updateModel.Approver?.Id)
                        {
                            await _notificationService.CreateNotificationsForLogApproverAssignment(new TOTLogNotificationViewModel(model, record));
                            if (previousStatus == Status.Pending)
                            {
                                dbModel.Status = Status.InProcess;
                            }
                        }
                        else
                        {
                            await _notificationService.CreateNotificationsForLogUpdation(new TOTLogNotificationViewModel(model, record));
                        }

                        await SetRequesterId(dbModel);
                        SetDelayReasonFields(dbModel, model);
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
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInUserId());
                mappedModel.CompanyId = (await _db.Users.Where(x => x.Id == mappedModel.EmployeeId).Select(x => x.CompanyId).FirstOrDefaultAsync()) ?? 0;
            }
            else if (mappedModel.EmployeeId == null || mappedModel.EmployeeId < 1)
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInUserId());
            }

        }

        private void SetDelayReasonFields(TOTLog mappedModel, IDelayType delayType)
        {
            if (mappedModel.DelayTypeId != null && mappedModel.DelayTypeId > 0)
            {
                if (delayType.DelayType.Identifier == DelayReasonCatalog.StartOfWork.ToString())
                {
                    mappedModel.ShiftDelayId = null;
                    mappedModel.ReworkDelayId = null;
                    mappedModel.OngoingWorkDelayId = null;
                }
                else if (delayType.DelayType.Identifier == DelayReasonCatalog.ShiftDelay.ToString())
                {
                    mappedModel.StartOfWorkDelayId = null;
                    mappedModel.ReworkDelayId = null;
                    mappedModel.OngoingWorkDelayId = null;
                }
                else if (delayType.DelayType.Identifier == DelayReasonCatalog.ReworkDelay.ToString())
                {
                    mappedModel.ShiftDelayId = null;
                    mappedModel.StartOfWorkDelayId = null;
                    mappedModel.OngoingWorkDelayId = null;
                }
                else if (delayType.DelayType.Identifier == DelayReasonCatalog.OngoingWork.ToString())
                {
                    mappedModel.ShiftDelayId = null;
                    mappedModel.StartOfWorkDelayId = null;
                    mappedModel.ReworkDelayId = null;
                }
            }
            else
            {
                mappedModel.ShiftDelayId = null;
                mappedModel.StartOfWorkDelayId = null;
                mappedModel.ReworkDelayId = null;
                mappedModel.OngoingWorkDelayId = null;
            }

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

        public async Task<IRepositoryResponse> GetDelayReason<BaseBriefVM>(IBaseSearchModel search)
        {
            try
            {

                List<Select2ViewModel> list = new List<Select2ViewModel>();
                foreach (DelayReasonCatalog delayReason in (DelayReasonCatalog[])Enum.GetValues(typeof(DelayReasonCatalog)))
                {
                    var field = typeof(DelayReasonCatalog).GetField(delayReason.ToString());
                    var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                    var displayName = displayAttribute?.Name ?? delayReason.ToString();
                    list.Add(new Select2ViewModel()
                    {
                        id = ((int)delayReason).ToString(),
                        text = displayName
                    });
                }

                if (list != null && list.Count > 0)
                {
                    //list = list.OrderBy(x => x.text).ToList();
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

        //private async Task<NotificationViewModel> GetNotificationModel(TOTLog model, NotificationEventTypeCatalog eventType)
        //{
        //    string userFullName = "";
        //    string userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        userFullName = await _db.Users.Where(x => x.Id == long.Parse(userId)).Select(x => x.FullName).FirstOrDefaultAsync();
        //    }
        //    return new NotificationViewModel()
        //    {
        //        LogId = model.Id,
        //        EntityId = model.Id,
        //        EventType = eventType,
        //        EntityType = NotificationEntityType.TOTLog,
        //        IdentifierKey = "Permit#",
        //        IdentifierValue = model.PermitNo,
        //        SendTo = model?.Approver?.Id.ToString(),
        //        Approver = userFullName,
        //        RequestorId = model.EmployeeId
        //    };
        //}



        public async Task<XLWorkbook> DownloadExcel(TOTLogSearchViewModel searchModel)
        {
            try
            {
                searchModel.IsExcelDownload = true;
                var response = await GetAll<TOTLogDetailViewModel>(searchModel);
                var logs = response as RepositoryResponseWithModel<PaginatedResultModel<TOTLogDetailViewModel>>;

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("TimeOnToolLogs");

                var columnHeaders = new List<string>
                {
                    "Company",
                    "Department",
                    "Requestor",
                    "Submitted Date",
                    "Submitted Time",
                    "Unit",
                    "Shift",
                    "Permit No",
                    "Permit Type",
                    "Description",
                    "Foreman",
                    "Twr",
                    //"Reason",
                    "DelayType",
                    "Start Of Work Delay",
                    "Shift Delay",
                    "Rework Delay",
                    "Head Count",
                    "Hours",
                    "Total Hours",
                    "Delay Description",
                    "Status",
                    "Approver"
                };
                AddColumnHeaders(worksheet, columnHeaders);

                AddDataRows(worksheet, logs.ReturnModel.Items);

                return workbook;
            }
            catch (Exception ex)
            {
                // handle exception
                return null;
            }
        }

        private void AddDataRows(IXLWorksheet worksheet, List<TOTLogDetailViewModel> items)
        {
            var row = 2;
            foreach (var item in items)
            {
                var logIndex = 0;

                worksheet.Cell(row, ++logIndex).Value = item.Company != null ? item.Company.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Department != null ? item.Department.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Employee != null ? item.Employee.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.FormattedCreatedDate;
                worksheet.Cell(row, ++logIndex).SetValue(item.FormattedCreatedTime);
                worksheet.Cell(row, ++logIndex).Value = item.Unit != null ? item.Unit.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Shift != null ? item.Shift.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.PermitNo;
                worksheet.Cell(row, ++logIndex).Value = item.PermitType != null ? item.PermitType.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.JobDescription;
                worksheet.Cell(row, ++logIndex).Value = item.Foreman;
                worksheet.Cell(row, ++logIndex).Value = item.Twr;
                //worksheet.Cell(row, ++logIndex).Value = item.ReasonForRequest != null ? item.ReasonForRequest.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.DelayType != null ? item.DelayType.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.StartOfWorkDelay != null ? item.StartOfWorkDelay.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.ShiftDelay != null ? item.ShiftDelay.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.ReworkDelay != null ? item.ReworkDelay.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.ManPowerAffected;
                worksheet.Cell(row, ++logIndex).Value = item.ManHours;
                worksheet.Cell(row, ++logIndex).Value = item.TotalHours;
                worksheet.Cell(row, ++logIndex).Value = item.DelayDescription;
                worksheet.Cell(row, ++logIndex).Value = item.Status;
                worksheet.Cell(row, ++logIndex).Value = item.Approver != null ? item.Approver.Name : "-";
                row++;
            }
        }
        private void AddColumnHeaders(IXLWorksheet worksheet, List<string> headers)
        {
            var row = worksheet.Row(1);
            //row.Style.Font.Bold = true; // uncomment it to bold the text of headers row 
            for (int i = 0; i < headers.Count; i++)
            {
                row.Cell(i + 1).Value = headers[i];
            }
        }


    }
}
