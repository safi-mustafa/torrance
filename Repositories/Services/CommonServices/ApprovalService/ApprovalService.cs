using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common;
using Pagination;
using Repositories.Services.CommonServices.ApprovalService.Interface;
using Repositories.Shared.UserInfoServices;
using ViewModels.Common;

namespace Repositories.Services.CommonServices.ApprovalService
{
    public class ApprovalService : IApprovalService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ApprovalService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfo;

        public ApprovalService(ToranceContext db, ILogger<ApprovalService> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfo)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            this._userInfo = userInfo;
        }

        public Task<IRepositoryResponse> Delete(long id, LogType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to get unapproved logs for Admin and Approvers
        ///     User won't get any result from this method.
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel model)
        {
            var search = model as ApprovalSearchViewModel;
            var userRoles = _userInfo.LoggedInUserRoles();
            var loggedInUserId = long.Parse(_userInfo.LoggedInUserId());
            var isApprover = userRoles.Contains("Approver");
            var isEmployee = userRoles.Contains("Employee");
            try
            {
                //commented for change in flow of assinging approver
                //List<string> approverAssociations = null;
                //if (isApprover)
                //{
                //    approverAssociations = await _db.ApproverAssociations.Where(x => x.IsDeleted == false && x.ApproverId == loggedInUserId).Select(x => x.DepartmentId + "-" + x.UnitId).Distinct().ToListAsync();
                //}

                var status = (Status?)((int?)search.Status);

                var totLogsQueryable = _db.TOTLogs
                    .Include(x => x.Employee)
                    .Include(x => x.Approver)
                    .Include(x => x.Unit)
                    .Where(x =>
                     x.IsDeleted == false
                     &&
                     (search.Employee.Id == 0 || search.Employee.Id == null || search.Employee.Id == x.EmployeeId)
                     &&
                     (search.Department.Id == 0 || search.Department.Id == null || search.Department.Id == x.DepartmentId)
                     &&
                     (search.Unit.Id == 0 || search.Unit.Id == null || search.Unit.Id == x.UnitId)
                     &&
                     (search.Company.Id == 0 || search.Company.Id == null || search.Company.Id == x.CompanyId)
                     &&
                     (search.Approver.Id == 0 || search.Approver.Id == null || search.Approver.Id == x.ApproverId)
                     //commented for change in flow of assinging approver
                     //&&
                     //(!isApprover || x.Approver == null || x.ApproverId == loggedInUserId)
                     &&
                     (!isApprover || x.ApproverId == loggedInUserId)
                     &&
                     (!isEmployee)
                     &&
                     (search.Type == null || search.Type == LogType.TimeOnTools)
                     &&
                     (status == null && (x.Status == Status.InProcess || x.Status == Status.Pending) || (status != null && x.Status == status))
                     &&
                     (string.IsNullOrEmpty(search.Search.value) || (x.Employee != null && x.Employee.FullName.Trim().ToLower().Contains(search.Search.value.ToLower().Trim())))
                     //commented for change in flow of assinging approver
                     //&&
                     //(isApprover == false || (approverAssociations != null && approverAssociations.Contains(x.DepartmentId.ToString() + "-" + x.UnitId.ToString())))
                     )
                     .Select(x =>
                        new ApprovalDetailViewModel
                        {
                            Id = x.Id,
                            Approver = x.Approver != null ? x.Approver.FullName : "-",
                            TotalHours = x.ManHours ?? 0,
                            Date = x.CreatedOn,
                            Status = x.Status,
                            Reason = x.ReasonForRequest != null ? x.ReasonForRequest.Name : "",
                            Unit = x.Unit != null ? x.Unit.Name : "",
                            Department = x.Department != null ? x.Department.Name : "",
                            Type = LogType.TimeOnTools,
                            Employee = x.Employee,
                            TotalCost = 0,
                            TotalHeadCount = x.ManPowerAffected,
                        }).OrderByDescending(x => x.Id).IgnoreQueryFilters().AsQueryable();

                var wrrLogsQueryable = _db.WRRLogs
                    .Include(x => x.Employee)
                    .Include(x => x.Approver)
                    .Include(x => x.Department)
                    .Include(x => x.Contractor)
                    .Include(x => x.Unit)
                    .Where(x =>
                        x.IsDeleted == false
                         &&
                        (search.Employee.Id == 0 || search.Employee.Id == null || search.Employee.Id == x.EmployeeId)
                        &&
                        (search.Approver.Id == 0 || search.Approver.Id == null || search.Approver.Id == x.ApproverId)
                        //commented for change in flow of assinging approver
                        //&&
                        //(!isApprover || x.Approver == null || x.ApproverId == loggedInUserId)
                        &&
                        (!isApprover || x.ApproverId == loggedInUserId)
                        &&
                        (search.Type == null || search.Type == LogType.WeldingRodRecord)
                        &&
                        (status == null && (x.Status == Status.InProcess || x.Status == Status.Pending) || (status != null && x.Status == status))
                        &&
                        (string.IsNullOrEmpty(search.Search.value) || (x.Employee != null && x.Employee.FullName.Trim().ToLower().Contains(search.Search.value.ToLower().Trim())))
                    //commented for change in flow of assinging approver
                    //&&
                    //(isApprover == false || (approverAssociations != null && approverAssociations.Contains(x.DepartmentId.ToString() + "-" + x.UnitId.ToString())))
                    )
                    .Select(x =>
                    new ApprovalDetailViewModel
                    {
                        Id = x.Id,
                        Approver = x.Approver != null ? x.Approver.FullName : "-",
                        TotalHours = 0,
                        Date = x.CreatedOn,
                        Status = x.Status,
                        Reason = "-",//x.ReasonForRequest != null ? x.ReasonForRequest.Name : "",
                        Unit = x.Unit != null ? x.Unit.Name : "",
                        Department = x.Department != null ? x.Department.Name : "",
                        Type = LogType.WeldingRodRecord,
                        Employee = x.Employee,
                        TotalCost = 0,
                        TotalHeadCount = 0
                    }).AsQueryable();

                var overrideLogsQueryable = _db.OverrideLogs
                    .Include(x => x.Employee)
                    .Include(x => x.Approver)
                    .Include(x => x.ReasonForRequest)
                    .Include(x => x.Contractor)
                    .Include(x => x.Unit)
                    .Where(x =>
                        x.IsDeleted == false
                        &&
                        (search.Employee.Id == 0 || search.Employee.Id == null || search.Employee.Id == x.EmployeeId)
                        &&
                        (search.Approver.Id == 0 || search.Approver.Id == null || search.Approver.Id == x.ApproverId)
                        //commented for change in flow of assinging approver
                        //&&
                        //(!isApprover || x.Approver == null || x.ApproverId == loggedInUserId)
                        &&
                        (!isApprover || x.ApproverId == loggedInUserId)
                        &&
                        (!isEmployee)
                        &&
                        (search.Type == null || search.Type == LogType.WeldingRodRecord)
                        &&
                        (status == null && (x.Status == Status.InProcess || x.Status == Status.Pending) || (status != null && x.Status == status))
                        &&
                        (string.IsNullOrEmpty(search.Search.value) || (x.Employee != null && x.Employee.FullName.Trim().ToLower().Contains(search.Search.value.ToLower().Trim())))
                    //commented for change in flow of assinging approver
                    //&&
                    //(isApprover == false || (approverAssociations != null && approverAssociations.Contains(x.DepartmentId.ToString() + "-" + x.UnitId.ToString())))
                    )
                    .Select(x =>
                    new ApprovalDetailViewModel
                    {
                        Id = x.Id,
                        Approver = x.Approver != null ? x.Approver.FullName : "-",
                        TotalHours = x.TotalHours,
                        Date = x.CreatedOn,
                        Status = x.Status,
                        Reason = x.ReasonForRequest != null ? x.ReasonForRequest.Name : "",
                        Unit = x.Unit != null ? x.Unit.Name : "",
                        Department = x.Department != null ? x.Department.Name : "",
                        Type = LogType.Override,
                        Employee = x.Employee,
                        TotalCost = x.TotalCost,
                        TotalHeadCount = x.TotalHeadCount
                    }).OrderByDescending(x => x.Id).IgnoreQueryFilters().AsQueryable();

                var Ids = overrideLogsQueryable.Select(x => x.Id).ToList();

                var logsQueryable = (totLogsQueryable.Concat(overrideLogsQueryable));
                logsQueryable = logsQueryable.Concat(wrrLogsQueryable);
                logsQueryable = logsQueryable.OrderByDescending(x => x.Date).AsQueryable();
                var result = await logsQueryable.Paginate(search);
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();
                    paginatedResult.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    paginatedResult._meta = result._meta;
                    paginatedResult._links = result._links;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for Approvals in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for Approval threw an exception.");
                return Response.BadRequestResponse(_response);
            }

        }
        public class LogDetailFromNotification
        {
            public long LogId { get; set; }
            public LogType LogType { get; set; }
            public long ApproverId { get; set; }
        }
        public async Task<IRepositoryResponse> GetLogIdAndTypeFromNotificationId(Guid notificationId)
        {
            var logDetails = new LogDetailFromNotification();
            try
            {
                var notification = await _db.Notifications.Where(x => x.Id == notificationId && x.Type == NotificationType.Email).FirstOrDefaultAsync();
                if (notification != null)
                {
                    logDetails.LogId = notification.EntityId ?? 0;
                    logDetails.LogType = notification.EntityType == NotificationEntityType.OverrideLog
                         ?
                         LogType.Override
                         :
                             notification.EntityType == NotificationEntityType.TOTLog
                             ?
                             LogType.TimeOnTools
                             :
                             LogType.WeldingRodRecord;
                    logDetails.ApproverId = long.Parse(notification.SendTo);

                    var response = new RepositoryResponseWithModel<LogDetailFromNotification> { ReturnModel = logDetails };
                    return response;
                }
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetLogIdAndTypeFromNotificationId() threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }
    }
}

