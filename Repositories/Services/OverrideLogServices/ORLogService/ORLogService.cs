using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using ClosedXML.Excel;
using DataLibrary;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Enums;
using Helpers.Extensions;
using Helpers.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Pagination;
using Repositories.Services.CommonServices.PossibleApproverService;
using Repositories.Shared;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using Select2.Model;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using ViewModels;
using ViewModels.Notification;
using ViewModels.OverrideLogs;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.WeldingRodRecord;
using ViewModels.WeldingRodRecord.Employee;

namespace Repositories.Services.OverrideLogServices.ORLogService
{
    public class ORLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<OverrideLog, CreateViewModel, UpdateViewModel, DetailViewModel>, IORLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IClippedAttachment, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IClippedAttachment, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ORLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly string _loggedInUserRole;
        private readonly long _loggedInUserId;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPossibleApproverService _possibleApproverService;
        private readonly IFileHelper _fileHelper;

        public ORLogService(
            ToranceContext db,
            ILogger<ORLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
            IMapper mapper,
            IRepositoryResponse response,
            IUserInfoService userInfoService,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor,
            IPossibleApproverService possibleApproverService,
            IFileHelper fileHelper) : base(db, logger, mapper, response, userInfoService, notificationService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _possibleApproverService = possibleApproverService;
            _fileHelper = fileHelper;
            _loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            _loggedInUserId = long.Parse(_userInfoService.LoggedInUserId() ?? "0"); ;
        }

        public override Expression<Func<OverrideLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as ORLogSearchViewModel;
            var status = (Status?)((int?)searchFilters.Status);
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole == RolesCatalog.CompanyManager.ToString() || searchFilters.IsExcelDownload)
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
                            (searchFilters.Requestor.Id == null || searchFilters.Requestor.Id == 0 || x.Employee.Id == searchFilters.Requestor.Id)
                            &&
                            (searchFilters.Approver.Id == null || searchFilters.Approver.Id == 0 || x.Approver.Id == searchFilters.Approver.Id)
                            &&
                            (searchFilters.Unit.Id == null || searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            //&&
                            //(searchFilters.OverrideType == null  || x.OverrideType == searchFilters.OverrideType)
                            &&
                            (searchFilters.Company.Id == null || searchFilters.Company.Id == 0 || x.Company.Id == searchFilters.Company.Id)
                            &&
                            (searchFilters.Department.Id == null || searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Shift.Id == null || searchFilters.Shift.Id == 0 || x.Shift.Id == searchFilters.Shift.Id)
                            &&
                            (
                                (_loggedInUserRole == RolesCatalog.SuperAdmin.ToString())
                                ||
                                 (_loggedInUserRole == RolesCatalog.Administrator.ToString())
                                ||
                                (_loggedInUserRole == RolesCatalog.Approver.ToString() && x.ApproverId == _loggedInUserId)
                                ||
                                (_loggedInUserRole == RolesCatalog.Employee.ToString() && x.EmployeeId == _loggedInUserId)
                                ||
                                (_loggedInUserRole == RolesCatalog.CompanyManager.ToString() && x.CompanyId == searchFilters.Company.Id)
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

        public async override Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var searchFilters = search as ORLogSearchViewModel;
                if (_loggedInUserRole == RolesCatalog.CompanyManager.ToString())
                    searchFilters.Company.Id = await _db.Users.Where(x => x.Id == _loggedInUserId).Select(x => x.CompanyId).FirstOrDefaultAsync();
                var filters = SetQueryFilter(search);
                var resultQuery = _db.Set<OverrideLog>()
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
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
                    if (searchFilters.IsExcelDownload)
                    {
                        await SetOverrideLogCosts(paginatedResult.Items as List<ORLogDetailViewModel>);
                    }
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
                    .Include(x => x.ReworkDelay)
                    .Include(x => x.ShiftDelay)
                    .Include(x => x.StartOfWorkDelay)
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
                                                   HeadCount = olc.HeadCount,
                                                   CraftSkill = new CraftSkillForORLogBriefViewModel()
                                                   {
                                                       Id = cs.Id,
                                                       Name = cs.Name,
                                                       STRate = cs.STRate,
                                                       OTRate = cs.OTRate,
                                                       DTRate = cs.DTRate
                                                   },
                                                   OverrideType = olc.OverrideType,
                                                   STHours = olc.STHours,
                                                   DTHours = olc.DTHours,
                                                   OTHours = olc.OTHours,
                                               }).ToListAsync();
                    mappedModel.ShiftDelay = mappedModel.ShiftDelay ?? new();
                    mappedModel.ReworkDelay = mappedModel.ReworkDelay ?? new();
                    mappedModel.StartOfWorkDelay = mappedModel.StartOfWorkDelay ?? new();
                    mappedModel.PossibleApprovers = await _possibleApproverService.GetPossibleApprovers(mappedModel.Unit.Id, mappedModel.Department.Id);
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
                    //save and attach clipped employees.
                    AddClippedEmployees(model, mappedModel);
                    mappedModel.Approver = null;
                    mappedModel.Id = 0;
                    await SetRequesterId(mappedModel);
                    await _db.Set<OverrideLog>().AddAsync(mappedModel);
                    mappedModel.TotalCost = await CalculateTotalCost(costs);
                    mappedModel.TotalHours = CalculateTotalHours(costs);
                    mappedModel.TotalHeadCount = CalculateTotalHeadCount(costs);
                    await _db.SaveChangesAsync();
                    await SetORLogCosts(costs, mappedModel.Id);
                    var notification = await GetNotificationModel(mappedModel, NotificationEventTypeCatalog.Created);
                    await _notificationService.CreateLogNotification(notification);
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
                            if (model.ClippedEmployees == null || model.ClippedEmployees?.File == null)
                            {
                                model.ClippedEmployees = new ClipEmployeeModifyViewModel();
                                model.ClippedEmployees.Url = record.ClippedEmployeesUrl;
                            }
                            var dbModel = _mapper.Map(model, record);
                            if (record.ApproverId != updateModel.Approver?.Id)
                            {
                                var notification = await GetNotificationModel(dbModel, NotificationEventTypeCatalog.Updated);
                                await _notificationService.Create(notification);
                            }

                            //save and attach clipped employees.

                            AddClippedEmployees(model, dbModel);
                            dbModel.Approver = null;
                            dbModel.TotalCost = await CalculateTotalCost(costs);
                            dbModel.TotalHours = CalculateTotalHours(costs);
                            dbModel.TotalHeadCount = CalculateTotalHeadCount(costs);
                            await SetRequesterId(dbModel);
                            await _db.SaveChangesAsync(model);
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

        private void AddClippedEmployees(IClippedAttachment model, OverrideLog mappedModel)
        {
            if (model.ClippedEmployees?.File != null)
            {
                mappedModel.ClippedEmployeesUrl = _fileHelper.Save(model.ClippedEmployees);

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
                await _db.SaveChangesAsync();
                if (overrideLogCost.Costs.Count() > 0)
                {
                    List<OverrideLogCost> list = new List<OverrideLogCost>();
                    foreach (var cost in overrideLogCost.Costs)
                    {
                        OverrideLogCost dbCost = new OverrideLogCost();
                        dbCost.STHours = cost.STHours;
                        dbCost.OTHours = cost.OTHours;
                        dbCost.DTHours = cost.DTHours;
                        dbCost.CraftSkillId = cost.CraftSkill.Id;
                        dbCost.HeadCount = cost.HeadCount;
                        dbCost.OverrideLogId = id;
                        if (dbCost.CraftSkillId > 0)
                            list.Add(dbCost);
                    }
                    if (list.Count > 0)
                    {
                        await _db.AddRangeAsync(list);
                        await _db.SaveChangesAsync();
                    }

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

                totalCost += (cost.OverrideHours ?? 0) * (cost.HeadCount ?? 0) * craftCost;
            }
            return totalCost;

        }

        private double CalculateTotalHours(IORLogCost overrideLogCost)
        {
            if (overrideLogCost.Costs == null || overrideLogCost.Costs.Count < 1)
            {
                return 0;
            }
            return overrideLogCost.Costs.Sum(x => x.OverrideHours ?? 0);

        }
        private double CalculateTotalHeadCount(IORLogCost overrideLogCost)
        {
            if (overrideLogCost.Costs == null || overrideLogCost.Costs.Count < 1)
            {
                return 0;
            }
            return overrideLogCost.Costs.Sum(x => x.HeadCount ?? 0);

        }

        private async Task<NotificationViewModel> GetNotificationModel(OverrideLog model, NotificationEventTypeCatalog eventType)
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
                EntityType = NotificationEntityType.OverrideLog,
                IdentifierKey = "PO#",
                IdentifierValue = model.PoNumber.ToString(),
                SendTo = model?.Approver?.Id.ToString(),
                User = userFullName
            };
        }

        private async Task SetOverrideLogCosts(List<ORLogDetailViewModel> overrideLogs)
        {
            List<ORLogCostViewModel> overrideLogCosts = new List<ORLogCostViewModel>();
            var batchSize = 500;
            var batches = overrideLogs.Select(x => x.Id).Chunk(batchSize);
            foreach (var batch in batches)
            {
                var costs = await (
                    from olc in _db.OverrideLogCost
                    join cs in _db.CraftSkills on olc.CraftSkillId equals cs.Id
                    where batch.Contains(olc.OverrideLogId)
                    select new ORLogCostViewModel
                    {
                        Id = olc.Id,
                        OverrideLogId = olc.OverrideLogId,
                        OverrideHours = olc.OverrideHours,
                        HeadCount = olc.HeadCount,
                        STHours = olc.STHours,
                        DTHours = olc.DTHours,
                        OTHours = olc.OTHours,
                        CraftSkill = new CraftSkillForORLogBriefViewModel()
                        {
                            Id = cs.Id,
                            Name = cs.Name,
                            STRate = cs.STRate,
                            OTRate = cs.OTRate,
                            DTRate = cs.DTRate
                        },
                        OverrideType = olc.OverrideType
                    }).ToListAsync();

                overrideLogCosts.AddRange(costs);

            }
            foreach (var overrideLog in overrideLogs)
            {
                overrideLog.Costs = overrideLogCosts.Where(x => x.OverrideLogId == overrideLog.Id).ToList();
            }
        }

        public async Task<XLWorkbook> DownloadExcel(ORLogSearchViewModel searchModel)
        {
            try
            {
                searchModel.IsExcelDownload = true;
                var response = await GetAll<ORLogDetailViewModel>(searchModel);

                var logs = response as RepositoryResponseWithModel<PaginatedResultModel<ORLogDetailViewModel>>;
                var httpContext = _httpContextAccessor.HttpContext;
                var baseUri = httpContext?.Request;
                var domainUrl = $"{baseUri?.Scheme}://{baseUri?.Host}";
                logs.ReturnModel.Items.ForEach(x => x.DomainUrl = domainUrl);
                // Create a new workbook
                var workbook = new XLWorkbook();
                var maxCostRows = logs.ReturnModel.Items.Max(x => x.Costs.Count);
                // Add a new worksheet to the workbook and set its name
                var overrideLogSheet = workbook.Worksheets.Add("OverrideLogs");
                //var overrideLogCostSheet = workbook.Worksheets.Add("OverrideLogCosts");

                SetExcelHeaders(overrideLogSheet, maxCostRows);

                var rowNumber = 1;
                var overrideCostIndex = 1;
                for (var l = 0; l < logs.ReturnModel.Items.Count(); l++)
                {
                    rowNumber = rowNumber + 1;
                    overrideLogSheet.Cell(rowNumber, 1).Value = logs.ReturnModel.Items[l].Company.Name;
                    overrideLogSheet.Cell(rowNumber, 2).Value = logs.ReturnModel.Items[l].Department.Name;
                    overrideLogSheet.Cell(rowNumber, 3).Value = logs.ReturnModel.Items[l].Employee.Name;
                    overrideLogSheet.Cell(rowNumber, 4).Value = logs.ReturnModel.Items[l].FormattedCreatedDate;
                    overrideLogSheet.Cell(rowNumber, 5).SetValue(logs.ReturnModel.Items[l].FormattedCreatedTime);
                    overrideLogSheet.Cell(rowNumber, 6).Value = logs.ReturnModel.Items[l].FormattedDateOfWorkCompleted;
                    overrideLogSheet.Cell(rowNumber, 7).Value = logs.ReturnModel.Items[l].WorkScope;
                    overrideLogSheet.Cell(rowNumber, 8).Value = logs.ReturnModel.Items[l].PoNumber;
                    overrideLogSheet.Cell(rowNumber, 9).Value = logs.ReturnModel.Items[l].Unit.Name;
                    overrideLogSheet.Cell(rowNumber, 10).Value = logs.ReturnModel.Items[l].Shift.Name;
                    overrideLogSheet.Cell(rowNumber, 11).Value = logs.ReturnModel.Items[l].Reason;
                    var checkCell = overrideLogSheet.Cell(rowNumber, 12);
                    checkCell.Value = logs.ReturnModel.Items[l].EmployeeNames;
                    //overrideLogSheet.Cell(rowNumber, 12).Value = logs.ReturnModel.Items[l].FormattedClippedEmployeeUrl;

                    var url = logs.ReturnModel.Items[l].FormattedClippedEmployeeUrl;

                    // Create a cell with the URL as a hyperlink
                    var cell = overrideLogSheet.Cell(rowNumber, 13);
                    if (!string.IsNullOrEmpty(url))
                    {
                        cell.Value = "link";
                        cell.Hyperlink = new XLHyperlink(new Uri(url));
                    }
                    else
                        cell.Value = "";

                    int currentColumn = 13;
                    for (int i = 0; i < maxCostRows; i++)
                    {
                        if (i > (logs.ReturnModel.Items[l].Costs.Count - 1))
                        {
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                            //overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = "-";
                        }
                        else
                        {
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Costs[i].CraftSkill.Name;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Costs[i].FormattedCraftRate;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Costs[i].OverrideHours;
                            overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Costs[i].OverrideType;
                            //overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Costs[i].HeadCount;
                            //overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Costs[i].FormattedCost;
                        }
                    }

                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].TotalHours;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].TotalHeadCount;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].TotalCost.ToString("C");
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].FormattedStatus;
                    overrideLogSheet.Cell(rowNumber, ++currentColumn).Value = logs.ReturnModel.Items[l].Approver.Name;

                    //for (var c = 0; c < logs.ReturnModel.Items[l].Costs.Count(); c++)
                    //{
                    //    overrideCostIndex = overrideCostIndex + 1;
                    //    overrideLogCostSheet.Cell($"A{overrideCostIndex}").Value = logs.ReturnModel.Items[l].PoNumber;
                    //    overrideLogCostSheet.Cell($"B{overrideCostIndex}").Value = logs.ReturnModel.Items[l].Costs[c].OverrideType;
                    //    overrideLogCostSheet.Cell($"C{overrideCostIndex}").Value = logs.ReturnModel.Items[l].Costs[c].CraftSkill.Name;
                    //    overrideLogCostSheet.Cell($"D{overrideCostIndex}").Value = logs.ReturnModel.Items[l].Costs[c].HeadCount;
                    //    overrideLogCostSheet.Cell($"E{overrideCostIndex}").Value = logs.ReturnModel.Items[l].Costs[c].OverrideHours;
                    //    overrideLogCostSheet.Cell($"f{overrideCostIndex}").Value = logs.ReturnModel.Items[l].Costs[c].FormattedCost;
                    //}
                }
                return workbook;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<bool> MergeCostsForCraft()
        {
            try
            {
                //getting all the costs with override not null from db
                var costs = await _db.OverrideLogCost.Where(x => x.OverrideType != null).ToListAsync();
                var mergedCosts = new List<OverrideLogCost>();
                foreach (var c in costs)
                {
                    //mapping the old cost on to new one, as per discussion, 
                    var mergedCost = new OverrideLogCost();
                    mergedCost.STHours = 0;
                    mergedCost.OTHours = 0;
                    mergedCost.DTHours = 0;
                    //mapping hours on the basis of Override Type
                    switch (c.OverrideType)
                    {
                        case OverrideTypeCatalog.ST: mergedCost.STHours = c.OverrideHours; break;
                        case OverrideTypeCatalog.OT: mergedCost.OTHours = c.OverrideHours; break;
                        case OverrideTypeCatalog.DT: mergedCost.DTHours = c.OverrideHours; break;
                    }
                    //mapping common fields
                    mergedCost.CraftSkillId = c.CraftSkillId;
                    mergedCost.OverrideLogId = c.OverrideLogId;
                    mergedCost.HeadCount = c.HeadCount;
                    mergedCosts.Add(mergedCost);
                }
                //removing old costs
                _db.RemoveRange(costs);
                //adding newly merged costs
                await _db.AddRangeAsync(mergedCosts);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception thrown in MergeCostForCraft()");
            }
            return false;
        }

        private void SetExcelHeaders(IXLWorksheet overrideLogSheet, long maxCostRows)
        {
            // overrideLogSheet.Row(1).Style.Font.Bold = true; // uncomment it to bold the text of headers row 
            overrideLogSheet.Cell(1, 1).Value = "Company";
            overrideLogSheet.Cell(1, 2).Value = "Department";
            overrideLogSheet.Cell(1, 3).Value = "Requester";
            overrideLogSheet.Cell(1, 4).Value = "Date Submitted";
            overrideLogSheet.Cell(1, 5).Value = "Time Submitted";
            overrideLogSheet.Cell(1, 6).Value = "Work Date";
            overrideLogSheet.Cell(1, 7).Value = "Workscope";
            overrideLogSheet.Cell(1, 8).Value = "PO Number";

            overrideLogSheet.Cell(1, 9).Value = "Unit";
            overrideLogSheet.Cell(1, 10).Value = "Shift";
            overrideLogSheet.Cell(1, 11).Value = "Override Reason";
            overrideLogSheet.Cell(1, 12).Value = "Employee Names";
            overrideLogSheet.Cell(1, 13).Value = "Clipped Employees";

            int currentColumn = 13;
            for (int i = 0; i < maxCostRows; i++)
            {
                //overrideLogSheet.Cell(1, ++currentColumn).Value = $"Override Type - {i + 1}";
                overrideLogSheet.Cell(1, ++currentColumn).Value = $"Craft Skill- {i + 1}";
                overrideLogSheet.Cell(1, ++currentColumn).Value = $"Craft Rate - {i + 1}";
                overrideLogSheet.Cell(1, ++currentColumn).Value = $"Override Hours - {i + 1}";
                overrideLogSheet.Cell(1, ++currentColumn).Value = $"OT Type - {i + 1}";
                //overrideLogSheet.Cell(1, ++currentColumn).Value = $"Head Count - {i + 1}";
                //overrideLogSheet.Cell(1, ++currentColumn).Value = $"Cost - {i + 1}";
            }

            currentColumn += 1;
            overrideLogSheet.Cell(1, currentColumn++).Value = "Total Hours";
            overrideLogSheet.Cell(1, currentColumn++).Value = "Total Head Count";
            overrideLogSheet.Cell(1, currentColumn++).Value = "Total Cost";
            overrideLogSheet.Cell(1, currentColumn++).Value = "Status";
            overrideLogSheet.Cell(1, currentColumn++).Value = "Approver";

            //overrideLogCostSheet.Cell($"A1").Value = "PO Number";
            //overrideLogCostSheet.Cell($"B1").Value = "Override Type";
            //overrideLogCostSheet.Cell($"C1").Value = "Craft";
            //overrideLogCostSheet.Cell($"D1").Value = "Head Count";
            //overrideLogCostSheet.Cell($"E1").Value = "Hours";
            //overrideLogCostSheet.Cell($"F1").Value = "Cost";

        }

    }
}
