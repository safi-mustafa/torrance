using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using ClosedXML.Excel;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common;
using Models.Common.Interfaces;
using Models.FCO;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using Repositories.Services.AppSettingServices.WRRLogService;
using Repositories.Services.CommonServices.PossibleApproverService;
using Repositories.Shared;
using Repositories.Shared.AttachmentService;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using System.Linq.Expressions;
using System.Security.Claims;
using ViewModels;
using ViewModels.Notification;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;

namespace Repositories.Services.AppSettingServices.FCOLogService
{
    public class FCOLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<FCOLog, CreateViewModel, UpdateViewModel, DetailViewModel>, IFCOLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, ISrNo, IFCOLogAttachment<AttachmentModifyViewModel>, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, ISrNo, IFCOLogAttachment<AttachmentModifyViewModel>, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<FCOLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPossibleApproverService _possibleApproverService;
        private readonly IAttachmentService<AttachmentModifyViewModel, AttachmentModifyViewModel, AttachmentModifyViewModel> _attachmentService;

        public FCOLogService(
                ToranceContext db,
                ILogger<FCOLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
                IMapper mapper,
                IRepositoryResponse response,
                IUserInfoService userInfoService,
                INotificationService notificationService,
                IHttpContextAccessor httpContextAccessor,
                IPossibleApproverService possibleApproverService,
                IAttachmentService<AttachmentModifyViewModel, AttachmentModifyViewModel, AttachmentModifyViewModel> attachmentService
            ) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _possibleApproverService = possibleApproverService;
            _attachmentService = attachmentService;
        }

        public override Expression<Func<FCOLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as FCOLogSearchViewModel;
            //searchFilters.OrderByColumn = "Status";
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var status = (Status?)((int?)searchFilters.Status);
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
                            (searchFilters.Department.Id == null || searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Unit.Id == null || searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            &&
                            (
                                (loggedInUserRole == "SuperAdmin")
                                ||
                                (loggedInUserRole == RolesCatalog.Administrator.ToString())
                                ||
                                (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
                            )
                            &&
                            (string.IsNullOrEmpty(searchFilters.Location) || x.Location.Trim().ToLower().Contains(searchFilters.Location.ToLower().Trim()))
                            &&
                            (searchFilters.Requestor.Id == null || searchFilters.Requestor.Id == 0 || x.Employee.Id == searchFilters.Requestor.Id)
                            //&&
                            //(searchFilters.Approver.Id == null || searchFilters.Approver.Id == 0 || x.Approver.Id == searchFilters.Approver.Id)
                            //&&
                            //(searchFilters.Company.Id == null || searchFilters.Company.Id == 0 || x.Company.Id == searchFilters.Company.Id)
                            &&
                            (searchFilters.SelectedIds == null || searchFilters.SelectedIds.Count <= 0 || searchFilters.SelectedIds.Contains(x.Id.ToString()) || x.Status == Status.Pending)
                            &&
                            (status == null || status == x.Status)
                            &&
                            (searchFilters.StatusNot == null || searchFilters.StatusNot != x.Status)
                            &&
                            x.Status != Status.Partial
                            &&
                            x.IsDeleted == false
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.FCOLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Employee)
                    .Include(x => x.FCOType)
                    .Include(x => x.FCOReason)
                    .Include(x => x.Contractor)
                    .Include(x => x.Company)
                    .Include(x => x.Contractor)
                    .Include(x => x.DesignatedCoordinator)
                    .Include(x => x.AreaExecutionLead)
                    .Include(x => x.BusinessTeamLeader)
                    .Include(x => x.Rejecter)
                    .Include(x => x.FCOComments)
                    .Include(x => x.FCOSections).ThenInclude(x => x.Craft)
                    .Where(x => x.Id == id && x.IsDeleted == false).IgnoreQueryFilters().FirstOrDefaultAsync();

                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<FCOLogDetailViewModel>(dbModel);
                    var attcs = await _db.Attachments.Where(x => x.EntityId == dbModel.Id && (x.EntityType == AttachmentEntityType.FCOLogPhoto || x.EntityType == AttachmentEntityType.FCOLogFile)).Select(x => new { EntityType = x.EntityType, Id = x.Id, Url = x.Url, Type = x.Type }).ToListAsync();
                    var photo = attcs.Where(x => x.EntityType == AttachmentEntityType.FCOLogPhoto).FirstOrDefault();
                    var file = attcs.Where(x => x.EntityType == AttachmentEntityType.FCOLogFile).FirstOrDefault();
                    mappedModel.Photo = new AttachmentModifyViewModel { EntityType = AttachmentEntityType.FCOLogPhoto, Url = photo?.Url, Id = photo?.Id ?? 0, Type = photo?.Type ?? "" };
                    mappedModel.File = new AttachmentModifyViewModel { EntityType = AttachmentEntityType.FCOLogFile, Url = file?.Url, Id = file?.Id ?? 0, Type = file?.Type ?? "" };
                    //mappedModel.TWRModel = new TWRViewModel(mappedModel.Twr);
                    //mappedModel.PossibleApprovers = await _possibleApproverService.GetPossibleApprovers(mappedModel.Unit.Id, mappedModel.Department.Id);
                    var response = new RepositoryResponseWithModel<FCOLogDetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for FCOLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for FCOLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }
        public override async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                //var check = await _db.FCOLogs.ToListAsync();

                var filters = SetQueryFilter(search);
                var resultQuery = _db.Set<FCOLog>()
                    .Include(x => x.Unit)
                    .Include(x => x.FCOComments)
                    .Include(x => x.Department)
                    .Include(x => x.Employee)
                    .Include(x => x.FCOType)
                    .Include(x => x.FCOReason)
                    .Include(x => x.Contractor)
                    .Include(x => x.Company)
                    .Include(x => x.Contractor)
                    .Include(x => x.DesignatedCoordinator)
                    .Include(x => x.AreaExecutionLead)
                    .Include(x => x.BusinessTeamLeader)
                    .Include(x => x.Rejecter)
                    .Include(x => x.FCOSections).ThenInclude(x => x.Craft)
                    .Where(filters)
                    .IgnoreQueryFilters();
                var result = await resultQuery.Paginate(search);
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();

                    paginatedResult.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    paginatedResult._meta = result._meta;
                    paginatedResult._links = result._links;
                    if (typeof(M).Equals(typeof(FCOLogRawReportViewModel)))
                    {
                        var maxCount = GetMaxSectionCount();
                        var reportList = paginatedResult.Items as List<FCOLogRawReportViewModel>;
                        reportList?.ForEach(x => x.SetSectionAsPerMaxCount(maxCount));
                        paginatedResult.Items = reportList as List<M>;
                    }
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(FCOLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(FCOLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }
        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var mappedModel = _mapper.Map<FCOLog>(model);
                    var srNo = await _db.FCOLogs.Where(x => x.UnitId == model.Unit.Id).CountAsync();
                    mappedModel.SrNo = srNo;
                    //setting requesterId
                    await SetRequesterId(mappedModel);
                    //setting approverId
                    //await SetApproverId(mappedModel);
                    mappedModel.DesignatedCoordinatorId = long.Parse(_userInfoService.LoggedInUserId());
                    await _db.Set<FCOLog>().AddAsync(mappedModel);
                    var result = await _db.SaveChangesAsync() > 0;
                    //saving Attachment
                    await AddAttachment(model.Photo, mappedModel.Id);
                    await AddAttachment(model.File, mappedModel.Id);

                    //var notification = await GetNotificationModel(mappedModel, NotificationEventTypeCatalog.Created);
                    //await _notificationService.CreateLogNotification(notification);
                    await transaction.CommitAsync();
                    var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                    return response;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, $"Exception thrown in Create method of {typeof(FCOLog).FullName}");
                    return Response.BadRequestResponse(_response);
                }
            }
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            try
            {
                var updateModel = model as FCOLogModifyViewModel;
                if (updateModel != null)
                {
                    var record = await _db.Set<FCOLog>().Include(x => x.FCOSections).Where(x => x.Id == updateModel.Id).FirstOrDefaultAsync();
                    if (record != null)
                    {
                        record.FCOSections?.Clear();
                        var dbModel = _mapper.Map(model, record);
                        //if (record.ApproverId != updateModel.Approver?.Id)
                        //{
                        //    var notification = await GetNotificationModel(dbModel, NotificationEventTypeCatalog.Updated);
                        //    await _notificationService.Create(notification);
                        //}
                        await SetRequesterId(dbModel);
                        await _db.SaveChangesAsync();
                        //update attachments
                        await AddAttachment(model.Photo, model.Id);
                        await AddAttachment(model.File, model.Id);
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                        return response;
                    }
                    _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(FCOLog).FullName} in Update()");
                }
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for {typeof(FCOLog).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task AddAttachment<T>(T model, long entityId) where T : AttachmentModifyViewModel
        {
            if (model.File != null)
            {
                var attc = await _db.Attachments.Where(x => x.EntityId == entityId && x.EntityType == model.EntityType).ToListAsync();
                if (attc.Count > 0)
                {
                    attc.ForEach(x => x.IsDeleted = true);
                    await _db.SaveChangesAsync();
                }
                model.Id = default;
                model.EntityId = entityId;
                model.EntityType = model.EntityType;
                model.Name = DateTime.Now.Ticks.ToString();
                var attachmentResponse = await _attachmentService.Create(model);
            }
        }

        private async Task SetRequesterId(FCOLog mappedModel)
        {
            var role = _userInfoService.LoggedInUserRole();
            if (role == "Employee")
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInUserId());
                //mappedModel.CompanyId = (await _db.Users.Where(x => x.Id == mappedModel.EmployeeId).Select(x => x.CompanyId).FirstOrDefaultAsync()) ?? 0;
            }
            else if (mappedModel.EmployeeId == null || mappedModel.EmployeeId < 1)
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInUserId());
            }

        }

        public async Task ApproveRecords(List<long> ids, bool Status, string comment, ApproverType approverType)
        {
            try
            {
                var logs = await _db.FCOLogs.Where(x => ids.Contains(x.Id)).ToListAsync();
                if (logs != null && logs.Count > 0)
                {
                    if (Status)
                    {
                        foreach (var log in logs)
                        {
                            if (approverType == ApproverType.AreaExecutionLead)
                            {
                                log.AreaExecutionLeadId = long.Parse(_userInfoService.LoggedInUserId());
                                log.AreaExecutionLeadApprovalDate = DateTime.Now;
                            }
                            else if (approverType == ApproverType.BusinessTeamLeader)
                            {
                                log.BusinessTeamLeaderId = long.Parse(_userInfoService.LoggedInUserId());
                                log.BusinessTeamLeaderApprovalDate = DateTime.Now;
                            }
                            var approvedByAreaExecutionLead = log.AreaExecutionLeadId != null && log.AreaExecutionLeadId > 0;
                            var approvedByBusinessTeamLeader = log.BusinessTeamLeaderId != null && log.BusinessTeamLeaderId > 0;
                            if (approvedByAreaExecutionLead && approvedByBusinessTeamLeader)
                            {
                                log.Status = Enums.Status.Approved;
                            }
                            else if (approvedByAreaExecutionLead || approvedByBusinessTeamLeader)
                            {
                                log.Status = Enums.Status.Partial;
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                await _db.AddAsync(new FCOComment { Comment = comment, FCOLogId = log.Id });
                            }
                        }
                    }
                    else
                    {
                        logs.ForEach(x => x.Status = Enums.Status.Rejected);
                    }
                    await _db.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ApproveRecords method for FCOLogs threw an exception.");
            }
        }

        private async Task SetApproverId(FCOLog mappedModel)
        {
            //mappedModel.ApproverId = long.Parse(_userInfoService.LoggedInUserId());
            mappedModel.Status = Status.Approved;
        }
        public async Task<bool> IsFCOLogEmailUnique(int id, string email)
        {
            //var check = await _db.FCOLogs.Where(x => x.Email == email && x.Id != id).CountAsync();
            //return check < 1;
            return true;
        }

        private async Task<NotificationViewModel> GetNotificationModel(FCOLog model, NotificationEventTypeCatalog eventType)
        {
            string userFullName = "";
            string userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                userFullName = await _db.Users.Where(x => x.Id == long.Parse(userId)).Select(x => x.FullName).FirstOrDefaultAsync();
            }
            return new NotificationViewModel()
            {
                LogId = model.Id,
                EntityId = model.Id,
                EventType = eventType,
                EntityType = NotificationEntityType.FCOLog,
                IdentifierKey = "FCO#",
                IdentifierValue = model.SrNo.ToString(),
                //SendTo = model?.Approver?.Id.ToString(),
                User = userFullName
            };
        }
        public async Task<IRepositoryResponse> SetApproveStatus(long id, Status status, bool isUnauthenticatedApproval = false, long approverId = 0, Guid notificationId = new Guid(), string comment = "", ApproverType approverType = 0)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var allowUnauthenticatedApproval = (isUnauthenticatedApproval && approverId > 0 && notificationId != new Guid());
                    allowUnauthenticatedApproval = allowUnauthenticatedApproval ? await _db.Notifications.AsNoTracking().AnyAsync(x => x.Id == notificationId && x.SendTo == approverId.ToString() && x.EntityId == id) : false;

                    if (isUnauthenticatedApproval == false || allowUnauthenticatedApproval)
                    {
                        var logRecord = await _db.FCOLogs.Where(x => x.Id == id).FirstOrDefaultAsync();
                        if (logRecord != null)
                        {
                            if (status == Status.Approved)
                            {

                                if (approverType == ApproverType.AreaExecutionLead)
                                {
                                    logRecord.AreaExecutionLeadId = long.Parse(_userInfoService.LoggedInUserId());
                                    logRecord.AreaExecutionLeadApprovalDate = DateTime.Now;
                                }
                                else if (approverType == ApproverType.BusinessTeamLeader)
                                {
                                    logRecord.BusinessTeamLeaderId = long.Parse(_userInfoService.LoggedInUserId());
                                    logRecord.BusinessTeamLeaderApprovalDate = DateTime.Now;
                                }
                                var approvedByAreaExecutionLead = logRecord.AreaExecutionLeadId != null && logRecord.AreaExecutionLeadId > 0;
                                var approvedByBusinessTeamLeader = logRecord.BusinessTeamLeaderId != null && logRecord.BusinessTeamLeaderId > 0;
                                if (approvedByAreaExecutionLead && approvedByBusinessTeamLeader)
                                {
                                    logRecord.Status = Enums.Status.Approved;
                                }
                                else if (approvedByAreaExecutionLead || approvedByBusinessTeamLeader)
                                {
                                    logRecord.Status = Enums.Status.Partial;
                                }
                            }
                            else
                            {
                                logRecord.RejecterId = long.Parse(_userInfoService.LoggedInUserId());
                                logRecord.RejecterDate = DateTime.Now;
                                logRecord.Status = Status.Rejected;
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                await _db.AddAsync(new FCOComment { Comment = comment, FCOLogId = logRecord.Id });
                            }

                            await _db.SaveChangesAsync();
                            string type = "";
                            string identifier = "";
                            string identifierKey = "";
                            NotificationEntityType notificationEntityType;

                            type = "FCO";
                            identifierKey = "FCO#";
                            identifier = (logRecord as FCOLog).SrNo.ToString();
                            notificationEntityType = NotificationEntityType.FCOLog;
                            var eventType = (status == Status.Approved ? NotificationEventTypeCatalog.Approved : NotificationEventTypeCatalog.Rejected);
                            string notificationTitle = $"{type} Log {status}";
                            //string notificationMessage = $"The {type} Log with {identifierKey}# ({identifier}) has been {status}";
                            var userId = await _db.Users.Where(x => x.Id == logRecord.EmployeeId).Select(x => x.Id).FirstOrDefaultAsync();
                            var notification = new NotificationViewModel()
                            {
                                LogId = logRecord.Id,
                                EntityId = logRecord.Id,
                                EventType = eventType,
                                Type = NotificationType.Push,
                                EntityType = notificationEntityType,
                                SendTo = userId.ToString() ?? "",
                                IdentifierKey = identifierKey,
                                IdentifierValue = identifier

                            };
                            await _notificationService.Create(notification);
                            var requestorId = logRecord.EmployeeId.ToString();
                            if (!string.IsNullOrEmpty(requestorId))
                            {
                                var notificationToRequestor = new NotificationViewModel()
                                {
                                    LogId = logRecord.Id,
                                    EntityId = logRecord.Id,
                                    EventType = eventType,
                                    Subject = $"{type} with {identifierKey}-{identifier} {eventType}",
                                    Type = NotificationType.Email,
                                    EntityType = notificationEntityType,
                                    SendTo = requestorId,
                                    IdentifierKey = identifierKey,
                                    IdentifierValue = identifier,
                                    User = await _db.Users.Where(x => x.Id == logRecord.EmployeeId).Select(x => x.FullName).FirstOrDefaultAsync()
                                };
                                await _notificationService.CreateProcessedLogNotification(notificationToRequestor, logRecord.AreaExecutionLeadId ?? logRecord.BusinessTeamLeaderId ?? 0);
                            }
                            await transaction.CommitAsync();
                            return _response;
                        }
                        _logger.LogWarning($"No record found for id:{id} for FCOLog in SetApproveStatus()");

                        await transaction.RollbackAsync();
                    }
                    return Response.NotFoundResponse(_response);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, $"ApproveRecords method for FCOLog threw an exception.");
                    return Response.BadRequestResponse(_response);
                }
            }
        }


        public async Task<XLWorkbook> DownloadExcel(FCOLogSearchViewModel searchModel)
        {
            try
            {
                searchModel.IsExcelDownload = true;
                var response = await GetAll<FCOLogDetailViewModel>(searchModel);

                var logs = response as RepositoryResponseWithModel<PaginatedResultModel<FCOLogDetailViewModel>>;

                // Create a new workbook
                var workbook = new XLWorkbook();
                var maxSectionRows = logs.ReturnModel.Items.Max(x => x.FCOSections.Count);
                // Add a new worksheet to the workbook and set its name
                var overrideLogSheet = workbook.Worksheets.Add("FCOLogs");

                SetExcelHeaders(overrideLogSheet, maxSectionRows);

                var rowNumber = 1;
                var overrideCostIndex = 1;
                for (var l = 0; l < logs.ReturnModel.Items.Count(); l++)
                {
                    rowNumber = rowNumber + 1;
                    overrideLogSheet.Cell(rowNumber, 1).Value = logs.ReturnModel.Items[l].SrNoFormatted;
                    overrideLogSheet.Cell(rowNumber, 2).Value = logs.ReturnModel.Items[l].AdditionalInformation;
                    overrideLogSheet.Cell(rowNumber, 3).Value = logs.ReturnModel.Items[l].EquipmentNumber;
                    overrideLogSheet.Cell(rowNumber, 4).Value = logs.ReturnModel.Items[l].Location;
                    overrideLogSheet.Cell(rowNumber, 5).Value = logs.ReturnModel.Items[l].PreTA;
                    overrideLogSheet.Cell(rowNumber, 6).Value = logs.ReturnModel.Items[l].ShutdownRequired;
                    overrideLogSheet.Cell(rowNumber, 7).Value = logs.ReturnModel.Items[l].ScaffoldRequired;
                    overrideLogSheet.Cell(rowNumber, 8).Value = logs.ReturnModel.Items[l].DescriptionOfFinding;
                    overrideLogSheet.Cell(rowNumber, 9).Value = logs.ReturnModel.Items[l].AnalysisOfAlternatives;
                    overrideLogSheet.Cell(rowNumber, 10).Value = logs.ReturnModel.Items[l].EquipmentFailureReport;
                    overrideLogSheet.Cell(rowNumber, 11).Value = logs.ReturnModel.Items[l].DrawingsAttached;
                    overrideLogSheet.Cell(rowNumber, 12).Value = logs.ReturnModel.Items[l].ScheduleImpact;
                    overrideLogSheet.Cell(rowNumber, 13).Value = logs.ReturnModel.Items[l].DaysImpacted;
                    overrideLogSheet.Cell(rowNumber, 14).Value = logs.ReturnModel.Items[l].DuringExecution;
                    overrideLogSheet.Cell(rowNumber, 15).Value = logs.ReturnModel.Items[l].DateFormatted;
                    overrideLogSheet.Cell(rowNumber, 16).Value = logs.ReturnModel.Items[l].ApprovalDateFormatted;
                    overrideLogSheet.Cell(rowNumber, 17).Value = logs.ReturnModel.Items[l].Contractor?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 18).Value = logs.ReturnModel.Items[l].Company?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 19).Value = logs.ReturnModel.Items[l].Employee?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 20).Value = logs.ReturnModel.Items[l].Department?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 21).Value = logs.ReturnModel.Items[l].Unit?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 22).Value = logs.ReturnModel.Items[l].FCOType?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 23).Value = logs.ReturnModel.Items[l].FCOReason?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 24).Value = logs.ReturnModel.Items[l].DesignatedCoordinator?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 25).Value = logs.ReturnModel.Items[l].AreaExecutionLead?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 26).Value = logs.ReturnModel.Items[l].AreaExecutionLeadApprovalDate;
                    overrideLogSheet.Cell(rowNumber, 27).Value = logs.ReturnModel.Items[l].BusinessTeamLeader?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 28).Value = logs.ReturnModel.Items[l].BusinessTeamLeaderApprovalDate;
                    overrideLogSheet.Cell(rowNumber, 29).Value = logs.ReturnModel.Items[l].Rejecter?.Name ?? "-";
                    overrideLogSheet.Cell(rowNumber, 30).Value = logs.ReturnModel.Items[l].RejecterDate;
                    overrideLogSheet.Cell(rowNumber, 31).Value = logs.ReturnModel.Items[l].TotalCostFormatted;
                    overrideLogSheet.Cell(rowNumber, 32).Value = logs.ReturnModel.Items[l].TotalHours;
                    overrideLogSheet.Cell(rowNumber, 33).Value = logs.ReturnModel.Items[l].TotalHeadCount;
                    overrideLogSheet.Cell(rowNumber, 34).Value = logs.ReturnModel.Items[l].MaterialName;
                    overrideLogSheet.Cell(rowNumber, 35).Value = logs.ReturnModel.Items[l].MaterialRate;
                    overrideLogSheet.Cell(rowNumber, 36).Value = logs.ReturnModel.Items[l].EquipmentName;
                    overrideLogSheet.Cell(rowNumber, 37).Value = logs.ReturnModel.Items[l].EquipmentRate;
                    overrideLogSheet.Cell(rowNumber, 38).Value = logs.ReturnModel.Items[l].ShopName;
                    overrideLogSheet.Cell(rowNumber, 39).Value = logs.ReturnModel.Items[l].ShopRate;

                    int currentColumn = 39;
                    for (int i = 0; i < maxSectionRows; i++)
                    {
                        if (i > (logs.ReturnModel.Items[l].FCOSections.Count - 1))
                        {
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                        }
                        else
                        {
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FCOSections[i].Name;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FCOSections[i].MN;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FCOSections[i].DU;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FCOSections[i].OverrideType;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FCOSections[i].Craft.Name;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FCOSections[i].RateFormatted;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FCOSections[i].Estimate;
                        }
                    }

                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Total;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Contingency;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Contingencies;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].SubTotal.ToString("C");
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].TotalLabor;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].TotalMaterial;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].TotalEquipment;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].TotalShop;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].SectionTotal;
                }
                return workbook;
            }
            catch (Exception ex)
            {

            }
            return null;
        }



        private void SetExcelHeaders(IXLWorksheet fcoLogSheet, long maxSectionRows)
        {
            // overrideLogSheet.Row(1).Style.Font.Bold = true; // uncomment it to bold the text of headers row 
            fcoLogSheet.Cell(1, 1).Value = "FCO#";
            fcoLogSheet.Cell(1, 2).Value = "Additional Information";
            fcoLogSheet.Cell(1, 3).Value = "Equipment Number";
            fcoLogSheet.Cell(1, 4).Value = "Location";
            fcoLogSheet.Cell(1, 5).Value = "Pre TA";
            fcoLogSheet.Cell(1, 6).Value = "Shutdown Required";
            fcoLogSheet.Cell(1, 7).Value = "Scaffold Required";
            fcoLogSheet.Cell(1, 8).Value = "Description Of Finding";
            fcoLogSheet.Cell(1, 9).Value = "Analysis Of Alternatives";
            fcoLogSheet.Cell(1, 10).Value = "Equipment Failure Report";
            fcoLogSheet.Cell(1, 11).Value = "Drawings Attached";
            fcoLogSheet.Cell(1, 12).Value = "Schedule Impact";
            fcoLogSheet.Cell(1, 13).Value = "Days Impact";
            fcoLogSheet.Cell(1, 14).Value = "During Execution";
            fcoLogSheet.Cell(1, 15).Value = "Date";
            fcoLogSheet.Cell(1, 16).Value = "Approval Date";
            fcoLogSheet.Cell(1, 17).Value = "Contractor";
            fcoLogSheet.Cell(1, 18).Value = "Company";
            fcoLogSheet.Cell(1, 19).Value = "Employee";
            fcoLogSheet.Cell(1, 20).Value = "Department";
            fcoLogSheet.Cell(1, 21).Value = "Unit";
            fcoLogSheet.Cell(1, 22).Value = "FCO Type";
            fcoLogSheet.Cell(1, 23).Value = "FCO Reason";
            fcoLogSheet.Cell(1, 24).Value = "Designated Coordinator";
            fcoLogSheet.Cell(1, 25).Value = "Area Execution Lead";
            fcoLogSheet.Cell(1, 26).Value = "Area Execution Lead Approval Date";
            fcoLogSheet.Cell(1, 27).Value = "Business Team Leader";
            fcoLogSheet.Cell(1, 28).Value = "Business Team Leader Approval Date";
            fcoLogSheet.Cell(1, 29).Value = "Rejecter";
            fcoLogSheet.Cell(1, 30).Value = "Rejecter Date";
            fcoLogSheet.Cell(1, 31).Value = "Total Cost";
            fcoLogSheet.Cell(1, 32).Value = "Total Hours";
            fcoLogSheet.Cell(1, 33).Value = "Total Head Count";
            fcoLogSheet.Cell(1, 34).Value = "Material Name";
            fcoLogSheet.Cell(1, 35).Value = "Material Rate";
            fcoLogSheet.Cell(1, 36).Value = "Equipment Name";
            fcoLogSheet.Cell(1, 37).Value = "Equipment Rate";
            fcoLogSheet.Cell(1, 38).Value = "Shop Name";
            fcoLogSheet.Cell(1, 39).Value = "Shop Rate";



            int currentColumn = 39;
            for (int i = 0; i < maxSectionRows; i++)
            {
                fcoLogSheet.Cell(1, ++currentColumn).Value = $"Name - {i + 1}";
                fcoLogSheet.Cell(1, ++currentColumn).Value = $"MN - {i + 1}";
                fcoLogSheet.Cell(1, ++currentColumn).Value = $"DU - {i + 1}";
                fcoLogSheet.Cell(1, ++currentColumn).Value = $"Type - {i + 1}";
                fcoLogSheet.Cell(1, ++currentColumn).Value = $"Craft - {i + 1}";
                fcoLogSheet.Cell(1, ++currentColumn).Value = $"Rate - {i + 1}";
                fcoLogSheet.Cell(1, ++currentColumn).Value = $"Estimate - {i + 1}";
            }

            currentColumn += 1;
            fcoLogSheet.Cell(1, currentColumn++).Value = "Total";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Contingency";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Contingencies";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Sub Total";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Total Labor";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Total Material";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Total Equipment";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Total Shop";
            fcoLogSheet.Cell(1, currentColumn++).Value = "Section Total";
        }

        public async Task<List<FCOCommentsViewModel>> GetFCOComments(long fcoId)
        {
            try
            {
                var comments = await (from c in _db.FCOComments
                                      join u in _db.Users on c.CreatedBy equals u.Id
                                      where c.FCOLogId == fcoId
                                      select new FCOCommentsViewModel
                                      {
                                          Comment = c.Comment,
                                          CommentedBy = u.FullName,
                                          CommentedDate = c.CreatedOn
                                      }).ToListAsync();
                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFCOComments method threw an exception.");
            }
            return null;
        }

        public long GetMaxSectionCount()
        {
            try
            {
                return _db.FCOSections.GroupBy(x => x.FCOLogId).ToList().Max(x => x.Count());
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
    }
}
