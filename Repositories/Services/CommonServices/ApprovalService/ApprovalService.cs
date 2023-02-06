﻿using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel model)
        {
            var search = model as ApprovalSearchViewModel;
            var userRoles = _userInfo.LoggedInUserRoles();
            var loggedInUserId = long.Parse(_userInfo.LoggedInUserId());
            var isApprover = userRoles.Contains("Approver");
            var isEmployee = userRoles.Contains("Employee");

            try
            {
                var totLogsQueryable = _db.TOTLogs
                    .Include(x => x.Employee)
                    .Include(x => x.Approver)
                    .Include(x => x.Unit)
                    .Where(x =>
                         (search.Employee.Id == 0 || search.Employee.Id == null || search.Employee.Id == x.EmployeeId)
                         &&
                         (!isApprover || (x.Approver != null && x.ApproverId == loggedInUserId))
                         &&
                         (!isEmployee)
                         &&
                         (search.Type == null || search.Type == LogType.TimeOnTools)
                         &&
                         (search.Status == null || search.Status == x.Status)
                         &&
                         (string.IsNullOrEmpty(search.Search.value) || (x.Employee != null && x.Employee.FirstName.Trim().ToLower().Contains(search.Search.value.ToLower().Trim())))
                     )
                     .Select(x =>
                        new ApprovalDetailViewModel
                        {
                            Id = x.Id,
                            Approver = x.Approver != null ? x.Approver.UserName : "",
                            TotalHours = x.ManHours ?? 0,
                            Date = x.CreatedOn,
                            Status = x.Status,
                            Reason = x.ReasonForRequest != null ? x.ReasonForRequest.Name : "",
                            Unit = x.Unit != null ? x.Unit.Name : "",
                            Type = LogType.TimeOnTools,
                            Employee = x.Employee
                        }).AsQueryable();

                //var wrrLogsQueryable = _db.WRRLogs
                //    .Include(x => x.Employee)
                //    .Include(x => x.Approver)
                //    .Include(x => x.Department)
                //    .Include(x => x.Contractor)
                //    .Include(x => x.Unit)
                //    .Where(x =>
                //         (search.Employee.Id == 0 || search.Employee.Id == null || search.Employee.Id == x.EmployeeId)
                //         &&
                //         (!userRoles.Contains("Approver") || (x.Approver != null && x.ApproverId == loggedInUserId))
                //         &&
                //         (search.Type == null || search.Type == LogType.WeldingRodRecord)
                //         &&
                //         (search.Status == null || search.Status == x.Status)
                //         &&
                //         (string.IsNullOrEmpty(search.Search.value) || (x.Employee != null && x.Employee.FirstName.Trim().ToLower().Contains(search.Search.value.ToLower().Trim())))
                //    )
                //    .Select(x =>
                //    new ApprovalDetailViewModel
                //    {
                //        Id = x.Id,
                //        Approver = x.Approver != null ? x.Approver.UserName : "",
                //        Contractor = x.Contractor != null ? x.Contractor.Name : "",
                //        Department = x.Department != null ? x.Department.Name : "",
                //        Date = x.CreatedOn,
                //        Status = x.Status,
                //        TWR = x.Twr,
                //        Unit = x.Unit != null ? x.Unit.Name : "",
                //        Type = LogType.WeldingRodRecord,
                //        Employee = x.Employee
                //    }).AsQueryable();

                var overrideLogsQueryable = _db.OverrideLogs
                    .Include(x => x.Requester)
                    .Include(x => x.Approver)
                    .Include(x => x.ReasonForRequest)
                    .Include(x => x.Contractor)
                    .Include(x => x.Unit)
                    .Where(x =>
                         (search.Employee.Id == 0 || search.Employee.Id == null || search.Employee.Id == x.RequesterId)
                         &&
                         (!isApprover || (x.Approver != null && x.ApproverId == loggedInUserId))
                         &&
                         (!isEmployee)
                         &&
                         (search.Type == null || search.Type == LogType.WeldingRodRecord)
                         &&
                         (search.Status == null || search.Status == x.Status)
                         &&
                         (string.IsNullOrEmpty(search.Search.value) || (x.Requester != null && x.Requester.FirstName.Trim().ToLower().Contains(search.Search.value.ToLower().Trim())))
                    )
                    .Select(x =>
                    new ApprovalDetailViewModel
                    {
                        Id = x.Id,
                        Approver = x.Approver != null ? x.Approver.UserName : "",
                        Date = x.CreatedOn,
                        Status = x.Status,
                        TotalHours = x.OverrideHours,
                        Reason = x.ReasonForRequest != null ? x.ReasonForRequest.Name : "",
                        Unit = x.Unit != null ? x.Unit.Name : "",
                        Type = LogType.Override,
                        Employee = x.Requester
                    }).AsQueryable();
                var logsQueryable = (totLogsQueryable.Concat(overrideLogsQueryable)).AsQueryable();
                var check = logsQueryable.ToQueryString();
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
    }
}

