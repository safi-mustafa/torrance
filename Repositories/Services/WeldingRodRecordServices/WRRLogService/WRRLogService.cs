using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using ClosedXML.Excel;
using DataLibrary;
using Enums;
using Helpers.ExcelReader;
using Helpers.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Services.CommonServices.PossibleApproverService;
using Repositories.Shared;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using System.Data.Common;
using System.Linq.Expressions;
using System.Security.Claims;
using ViewModels.Notification;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.Shared.Interfaces;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.AppSettingServices.WRRLogService
{
    public class WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<WRRLog, CreateViewModel, UpdateViewModel, DetailViewModel>, IWRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IWRRLogNotificationViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IWRRLogNotificationViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPossibleApproverService _possibleApproverService;
        private readonly IHostingEnvironment _env;
        private readonly string _loggedInUserRole;
        private readonly long _loggedInUserId;

        public WRRLogService(
                ToranceContext db,
                ILogger<WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
                IMapper mapper,
                IRepositoryResponse response,
                IUserInfoService userInfoService,
                INotificationService notificationService,
                IHttpContextAccessor httpContextAccessor,
                IPossibleApproverService possibleApproverService,
                IHostingEnvironment env
            ) : base(db, logger, mapper, response, userInfoService, notificationService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _possibleApproverService = possibleApproverService;
            _env = env;
            _loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            _loggedInUserId = long.Parse(_userInfoService.LoggedInUserId() ?? "0"); ;
        }

        public override Expression<Func<WRRLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as WRRLogSearchViewModel;
            //searchFilters.OrderByColumn = "Status";
            var status = (Status?)((int?)searchFilters.Status);
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole == RolesCatalog.CompanyManager.ToString() || searchFilters.IsExcelDownload)
            {
                searchFilters.StatusNot = null;
            }
            else
            {
                searchFilters.StatusNot.Add(Status.Pending);
                searchFilters.StatusNot.Add(Status.InProcess);
            }
            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Approver.FullName.ToLower().Contains(searchFilters.Search.value.ToLower()) || x.Employee.FullName.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Email) || x.Email.ToLower().Contains(searchFilters.Email.ToLower()))
                            &&
                            (searchFilters.Department.Id == null || searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Unit.Id == null || searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            &&
                            (
                                (_loggedInUserRole == RolesCatalog.SuperAdmin.ToString())
                                ||
                                (_loggedInUserRole == RolesCatalog.Administrator.ToString())
                                ||
                                (_loggedInUserRole == RolesCatalog.Approver.ToString() && (x.ApproverId == _loggedInUserId || x.EmployeeId == _loggedInUserId))
                                ||
                                (_loggedInUserRole == RolesCatalog.Employee.ToString() && x.EmployeeId == _loggedInUserId)
                            )
                            &&
                            (searchFilters.Location.Id == null || searchFilters.Location.Id == 0 || x.Location.Id == searchFilters.Location.Id)
                            &&
                            (searchFilters.Requestor.Id == null || searchFilters.Requestor.Id == 0 || x.Employee.Id == searchFilters.Requestor.Id)
                            &&
                            (searchFilters.Approver.Id == null || searchFilters.Approver.Id == 0 || x.Approver.Id == searchFilters.Approver.Id)
                            &&
                            (searchFilters.Company.Id == null || searchFilters.Company.Id == 0 || x.Company.Id == searchFilters.Company.Id)
                            &&
                            (searchFilters.SelectedIds == null || searchFilters.SelectedIds.Count <= 0 || searchFilters.SelectedIds.Contains(x.Id.ToString()) || x.Status == Status.Pending)
                            &&
                            (status == null || status == x.Status)
                            &&
                            (searchFilters.StatusNot == null || searchFilters.StatusNot.Count == 0 || !searchFilters.StatusNot.Contains(x.Status) || (_loggedInUserRole == RolesCatalog.Approver.ToString() && x.EmployeeId == _loggedInUserId))
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
                var isApprover = _userInfoService.LoggedInUserRole() == "Approver";
                var loggedInUserId = _userInfoService.LoggedInUserId();
                var parsedLoggedInUser = long.Parse(!string.IsNullOrEmpty(loggedInUserId) ? loggedInUserId : "0");
                var dbModel = await _db.WRRLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Location)
                    .Include(x => x.Employee)
                    .Include(x => x.RodType)
                    .Include(x => x.WeldMethod)
                    .Include(x => x.Approver)
                    .Include(x => x.Contractor)
                    .Include(x => x.Company)
                    .Where(x =>
                        x.Id == id
                        &&
                        x.IsDeleted == false
                        &&
                        (
                            isApprover == false
                            ||
                            (parsedLoggedInUser > 0 && (x.ApproverId == parsedLoggedInUser || x.EmployeeId == parsedLoggedInUser))
                        )
                    ).IgnoreQueryFilters().FirstOrDefaultAsync();

                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<WRRLogDetailViewModel>(dbModel);
                    mappedModel.TWRModel = new TWRViewModel(mappedModel.Twr);
                    mappedModel.LoggedInUserRole = _loggedInUserRole;
                    mappedModel.LoggedInUserId = _loggedInUserId;
                    //mappedModel.PossibleApprovers = await _possibleApproverService.GetPossibleApprovers(mappedModel.Unit.Id, mappedModel.Department.Id);
                    var response = new RepositoryResponseWithModel<WRRLogDetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for WRRLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for WRRLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }
        public override async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var filters = SetQueryFilter(search);
                var resultQuery = _db.Set<WRRLog>()
                   .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Location)
                    .Include(x => x.Employee)
                    .Include(x => x.RodType)
                    .Include(x => x.WeldMethod)
                    .Include(x => x.Approver)
                    .Include(x => x.Contractor)
                    .Include(x => x.Company)
                    .Where(filters).IgnoreQueryFilters();
                var result = await resultQuery.Paginate(search);
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();
                    paginatedResult.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    foreach (var item in paginatedResult.Items)
                    {
                        var logItem = item as LogCommonDetailViewModel;
                        logItem.LoggedInUserRole = _loggedInUserRole;
                        logItem.LoggedInUserId = _loggedInUserId;
                    }
                    paginatedResult._meta = result._meta;
                    paginatedResult._links = result._links;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(WRRLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(WRRLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }
        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var mappedModel = _mapper.Map<WRRLog>(model);
                    await SetRequesterId(mappedModel);
                    await _db.Set<WRRLog>().AddAsync(mappedModel);
                    var result = await _db.SaveChangesAsync() > 0;
                    await _notificationService.CreateNotificationsForLogCreation(new WRRLogNotificationViewModel(model, mappedModel));
                    if (mappedModel.ApproverId != null && mappedModel.ApproverId > 0)
                    {
                        await _notificationService.CreateNotificationsForLogApproverAssignment(new WRRLogNotificationViewModel(model, mappedModel));
                    }
                    await transaction.CommitAsync();
                    var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                    return response;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, $"Exception thrown in Create method of {typeof(WRRLog).FullName}");
                    return Response.BadRequestResponse(_response);
                }
            }
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            try
            {
                var updateModel = model as WRRLogModifyViewModel;
                if (updateModel != null)
                {
                    var record = await _db.Set<WRRLog>().FindAsync(updateModel?.Id);
                    if (record != null)
                    {

                        var previousApproverId = record.ApproverId;
                        var previousStatus = record.Status;
                        var dbModel = _mapper.Map(model, record);
                        dbModel.Status = previousStatus;
                        if (previousApproverId != updateModel.Approver?.Id)
                        {
                            if (record.ApproverId != null && record.ApproverId > 0)
                            {
                                await _notificationService.CreateNotificationsForLogApproverAssignment(new WRRLogNotificationViewModel(model, record));
                            }

                            //if (previousStatus == Status.Pending)
                            //{
                            //    dbModel.Status = Status.InProcess;
                            //}
                        }
                        else
                        {
                            if (previousStatus == Status.Pending || previousStatus == Status.InProcess)
                            {
                                await _notificationService.CreateNotificationsForLogUpdation(new WRRLogNotificationViewModel(model, record));
                            }
                        }

                        await SetRequesterId(dbModel);
                        await _db.SaveChangesAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                        return response;
                    }
                    _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(WRRLog).FullName} in Update()");
                }
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for {typeof(WRRLog).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task SetRequesterId(WRRLog mappedModel)
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
        public async Task<bool> IsWRRLogEmailUnique(int id, string email)
        {
            var check = await _db.WRRLogs.Where(x => x.Email == email && x.Id != id).CountAsync();
            return check < 1;
        }

        //private async Task<NotificationViewModel> GetNotificationModel(WRRLog model, NotificationEventTypeCatalog eventType)
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
        //        EntityType = NotificationEntityType.WRRLog,
        //        IdentifierKey = "TWR#",
        //        IdentifierValue = model.Twr,
        //        SendTo = model?.Approver?.Id.ToString(),
        //        Approver = userFullName,
        //        RequestorId = model.EmployeeId
        //    };
        //}

        public async Task<XLWorkbook> DownloadExcel(WRRLogSearchViewModel searchModel)
        {
            try
            {
                searchModel.IsExcelDownload = true;
                var response = await GetAll<WRRLogDetailViewModel>(searchModel);
                var logs = response as RepositoryResponseWithModel<PaginatedResultModel<WRRLogDetailViewModel>>;

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("WeldingRodRecordLogs");
                LogExcelHelper.AddLogo(worksheet, _env);

                var columnHeaders = new List<string>
                {
                    "Company",
                    "Department",
                    "Requestor",
                    "Submitted Date",
                    "Submitted Time",
                    "Unit",
                    "Calibration Date",
                    "Fume Control Used",
                    "Rod Type",
                    "Twr",
                    "Weld Method",
                    "Checked Out",
                    "Location",
                    "Rod Checked Out lbs",
                    "Rod Returned Waste lbs",
                    "Returned",
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

        private void AddDataRows(IXLWorksheet worksheet, List<WRRLogDetailViewModel> items)
        {
            var row = 3;
            foreach (var item in items)
            {
                var logIndex = 0;

                worksheet.Cell(row, ++logIndex).Value = item.Company != null ? item.Company.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Department != null ? item.Department.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Employee != null ? item.Employee.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.FormattedCreatedDate;
                worksheet.Cell(row, ++logIndex).SetValue(item.FormattedCreatedTime);
                worksheet.Cell(row, ++logIndex).Value = item.Unit != null ? item.Unit.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.FormattedCalibrationDate;
                worksheet.Cell(row, ++logIndex).Value = item.FormattedFumeControlUsed;
                worksheet.Cell(row, ++logIndex).Value = item.RodType != null ? item.RodType.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Twr;
                worksheet.Cell(row, ++logIndex).Value = item.WeldMethod != null ? item.WeldMethod.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.RodCheckedOut;
                worksheet.Cell(row, ++logIndex).Value = item.Location != null ? item.Location.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.RodCheckedOutLbs;
                worksheet.Cell(row, ++logIndex).Value = item.RodReturnedWasteLbs;
                worksheet.Cell(row, ++logIndex).Value = item.DateRodReturned;
                worksheet.Cell(row, ++logIndex).Value = item.FormattedStatus;
                worksheet.Cell(row, ++logIndex).Value = item.Approver != null ? item.Approver.Name : "-";
                row++;
            }
        }
        private void AddColumnHeaders(IXLWorksheet worksheet, List<string> headers)
        {
            var row = worksheet.Row(2);
            worksheet.Row(2).Style.Font.Bold = true; // uncomment it to bold the text of headers row 
            for (int i = 0; i < headers.Count; i++)
            {
                row.Cell(i + 1).Value = headers[i];
            }
        }
    }
}
