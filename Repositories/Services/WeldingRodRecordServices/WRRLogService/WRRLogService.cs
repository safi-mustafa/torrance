using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Shared;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using System.Data.Common;
using System.Linq.Expressions;
using ViewModels.Notification;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.AppSettingServices.WRRLogService
{
    public class WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<WRRLog, CreateViewModel, UpdateViewModel, DetailViewModel>, IWRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService<NotificationModifyViewModel, NotificationModifyViewModel, NotificationModifyViewModel> _notificationService;

        public WRRLogService(
                ToranceContext db,
                ILogger<WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
                IMapper mapper,
                IRepositoryResponse response,
                IUserInfoService userInfoService,
                INotificationService<NotificationModifyViewModel, NotificationModifyViewModel, NotificationModifyViewModel> notificationService
            ) : base(db, logger, mapper, response, userInfoService, notificationService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
        }

        public override Expression<Func<WRRLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as WRRLogSearchViewModel;
            searchFilters.OrderByColumn = "Status";
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var status = (Status?)((int?)searchFilters.Status);
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Email.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Email) || x.Email.ToLower().Contains(searchFilters.Email.ToLower()))
                            &&
                            (searchFilters.Department.Id == null || searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Unit.Id == null || searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            &&
                            (
                                (loggedInUserRole == "SuperAdmin")
                                ||
                                (loggedInUserRole == "Approver" && x.ApproverId == parsedLoggedInId)
                                ||
                                (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
                            )
                            &&
                            (searchFilters.Location.Id == null || searchFilters.Location.Id == 0 || x.Location.Id == searchFilters.Location.Id)
                            &&
                            (status == null || status == x.Status)
                            &&
                            (searchFilters.StatusNot == null || searchFilters.StatusNot != x.Status)
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
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
                    .Where(x => x.Id == id).FirstOrDefaultAsync();

                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<WRRLogDetailViewModel>(dbModel);
                    mappedModel.TWRModel = new TWRViewModel(mappedModel.Twr);
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
                    string notificationTitle = "WRR Log Created";
                    string notificationMessage = $"A new WRR Log with TWR# ({mappedModel.Twr}) has been created";
                    await _notificationService.Create(new NotificationModifyViewModel(mappedModel.Id, typeof(WRRLog), mappedModel.ApproverId?.ToString() ?? "", notificationTitle, notificationMessage, NotificationType.Push));
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
                        if (record.ApproverId != updateModel.Approver.Id)
                        {
                            string notificationTitle = "WRR Log Updated";
                            string notificationMessage = $"The WRR Log with TWR# ({updateModel.Twr}) has been updated";
                            await _notificationService.Create(new NotificationModifyViewModel(record.Id, typeof(WRRLog), updateModel.Approver.Id?.ToString() ?? "", notificationTitle, notificationMessage, NotificationType.Push));
                        }
                        var dbModel = _mapper.Map(model, record);
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
            }
            mappedModel.CompanyId = (await _db.Users.Where(x => x.Id == mappedModel.EmployeeId).Select(x => x.CompanyId).FirstOrDefaultAsync()) ?? 0;
        }
        public async Task<bool> IsWRRLogEmailUnique(int id, string email)
        {
            var check = await _db.WRRLogs.Where(x => x.Email == email && x.Id != id).CountAsync();
            return check < 1;
        }
    }
}
