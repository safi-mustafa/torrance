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
                    //.Include(x => x.Approver)
                    .Include(x => x.Company)
                    .Include(x => x.Contractor)
                    .Include(x => x.DesignatedCoordinator)
                    .Include(x => x.FCOSections).ThenInclude(x => x.Craft)
                    .Where(x => x.Id == id && x.IsDeleted == false).IgnoreQueryFilters().FirstOrDefaultAsync();

                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<FCOLogDetailViewModel>(dbModel);
                    var attcs = await _db.Attachments.Where(x => x.EntityId == dbModel.Id && (x.EntityType == AttachmentEntityType.FCOLogPhoto || x.EntityType == AttachmentEntityType.FCOLogFile)).Select(x => new { EntityType = x.EntityType, Id = x.Id, Url = x.Url }).ToListAsync();
                    var photo = attcs.Where(x => x.EntityType == AttachmentEntityType.FCOLogPhoto).FirstOrDefault();
                    var file = attcs.Where(x => x.EntityType == AttachmentEntityType.FCOLogFile).FirstOrDefault();
                    mappedModel.Photo = new AttachmentModifyViewModel { EntityType = AttachmentEntityType.FCOLogPhoto, Url = photo?.Url, Id = photo?.Id ?? 0 };
                    mappedModel.File = new AttachmentModifyViewModel { EntityType = AttachmentEntityType.FCOLogFile, Url = file?.Url, Id = file?.Id ?? 0 };
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
                    .Include(x => x.Department)
                    .Include(x => x.Employee)
                    .Include(x => x.FCOType)
                    .Include(x => x.FCOReason)
                    .Include(x => x.Contractor)
                    //.Include(x => x.Approver)
                    .Include(x => x.Company)
                    .Include(x => x.Contractor)
                    .Include(x => x.DesignatedCoordinator)
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
                IdentifierKey = "TWR#",
                //IdentifierValue = model.Twr,
                //SendTo = model?.Approver?.Id.ToString(),
                User = userFullName
            };
        }

        public async Task<XLWorkbook> DownloadExcel(FCOLogSearchViewModel searchModel)
        {
            try
            {
                searchModel.IsExcelDownload = true;
                var response = await GetAll<FCOLogDetailViewModel>(searchModel);
                var logs = response as RepositoryResponseWithModel<PaginatedResultModel<FCOLogDetailViewModel>>;

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("WeldingRodRecordLogs");

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

        private void AddDataRows(IXLWorksheet worksheet, List<FCOLogDetailViewModel> items)
        {
            var row = 2;
            foreach (var item in items)
            {
                var logIndex = 0;

                //worksheet.Cell(row, ++logIndex).Value = item.Company != null ? item.Company.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Department != null ? item.Department.Name : "-";
                worksheet.Cell(row, ++logIndex).Value = item.Employee != null ? item.Employee.Name : "-";
                //worksheet.Cell(row, ++logIndex).Value = item.FormattedCreatedDate;
                //worksheet.Cell(row, ++logIndex).SetValue(item.FormattedCreatedTime);
                //worksheet.Cell(row, ++logIndex).Value = item.Unit != null ? item.Unit.Name : "-";
                //worksheet.Cell(row, ++logIndex).Value = item.FormattedCalibrationDate;
                //worksheet.Cell(row, ++logIndex).Value = item.FumeControlUsed;
                //worksheet.Cell(row, ++logIndex).Value = item.RodType != null ? item.RodType.Name : "-";
                //worksheet.Cell(row, ++logIndex).Value = item.Twr;
                //worksheet.Cell(row, ++logIndex).Value = item.WeldMethod != null ? item.WeldMethod.Name : "-";
                //worksheet.Cell(row, ++logIndex).Value = item.RodCheckedOut;
                //worksheet.Cell(row, ++logIndex).Value = item.Location != null ? item.Location.Name : "-";
                //worksheet.Cell(row, ++logIndex).Value = item.RodCheckedOutLbs;
                //worksheet.Cell(row, ++logIndex).Value = item.RodReturnedWasteLbs;
                //worksheet.Cell(row, ++logIndex).Value = item.DateRodReturned;
                worksheet.Cell(row, ++logIndex).Value = item.Status;
                worksheet.Cell(row, ++logIndex).Value = item.Approver != null ? item.Approver.Name : "-";
                row++;
            }
        }
        private void AddColumnHeaders(IXLWorksheet worksheet, List<string> headers)
        {
            var row = worksheet.Row(1);
            //  row.Style.Font.Bold = true; // uncomment it to bold the text of headers row 
            for (int i = 0; i < headers.Count; i++)
            {
                row.Cell(i + 1).Value = headers[i];
            }
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
    }
}
