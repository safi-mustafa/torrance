using Microsoft.AspNetCore.Mvc;
using Select2.Model;
using Select2;
using Repositories.Interfaces;
using Pagination;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.Shared;
using AutoMapper;
using Models.Common.Interfaces;
using Web.Helpers;
using Centangle.Common.ResponseHelpers.Models;
using Repositories.Shared.Interfaces;
using Enums;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Spreadsheet;
using Repositories.Shared.UserInfoServices;

namespace Web.Controllers
{

    public abstract class ApproveBaseController<Service, CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : CrudBaseController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
        where Service : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
    {
        private readonly Service _service;
        private readonly IBaseApprove _approveService;
        private readonly ILogger<Controller> _logger;
        private readonly string _controllerName;
        private readonly string _title;
        private readonly IUserInfoService _userInfo;
        private readonly bool _useSameUpdateViews;
        private readonly IMapper _mapper;
        public ApproveBaseController(Service service, ILogger<Controller> logger, IMapper mapper, string controllerName, string title, IUserInfoService userInfo, bool hideCreateButton = false, bool useSameUpdateViews = true) : base(service, logger, mapper, controllerName, title, hideCreateButton, useSameUpdateViews)
        {
            _service = service;
            _logger = logger;
            _controllerName = controllerName;
            _title = title;
            _userInfo = userInfo;
            _useSameUpdateViews = useSameUpdateViews;
            _mapper = mapper;
        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            var loggedInUserRole = _userInfo.LoggedInUserRole();
            var html = "";
            if (loggedInUserRole == RolesCatalog.Employee.ToString() || loggedInUserRole == RolesCatalog.CompanyManager.ToString())
            {
                html += @"
                    <div class=""p-2 row"">
                        <span class=""badge Submitted m-1""> </span>
                        <span class=""stat-name"">Pending</span>
                    </div>";
            }

            html += @"
                    <div class=""p-2 row"">
                        <span class=""badge Approved m-1""> </span>
                        <span class=""stat-name"">Approved</span>
                    </div>
                    <div class=""m-2 row"">
                        <span class=""badge Rejected m-1""> </span>
                        <span class=""stat-name"">Rejected</span>
                    </div>
                    <div class=""m-2 row"">
                        <span class=""badge Archived m-1""> </span>
                        <span class=""stat-name"">Archived</span>
                    </div>";
            vm.DataTableHeaderHtml = html;
            vm.IsResponsiveDatatable = false;
            bool canAddLogs = false;
            var canAddLogsClaim = User.FindFirst("CanAddLogs");
            bool.TryParse(canAddLogsClaim?.Value, out canAddLogs);
            vm.HideCreateButton = !(_userInfo.LoggedInUserRoles().Contains("Administrator") || _userInfo.LoggedInUserRoles().Contains("SuperAdmin") || _userInfo.LoggedInUserRoles().Contains("Employee") || canAddLogs);
            return vm;
        }
        public async Task<ActionResult> ApproveRecords(List<long> Ids, bool Status)
        {
            try
            {
                await _service.ApproveRecords(Ids, Status);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Activity _ApproveTimesheets method threw an exception, Message: {ex.Message}");
                return null;
            }
        }

        public async Task<ActionResult> GetApprovedRecordIds()
        {
            try
            {
                var result = await _service.GetApprovedRecordIds();
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Activity _CheckProjectCBOs method threw an exception, Message: {ex.Message}");
                return null;
            }

        }
        [AllowAnonymous]
        public async Task<bool> ApproveStatus(long id, Status status, bool isUnauthenticatedApproval, long approverId, Guid notificationId)
        {
            try
            {
                var response = await _service.SetApproveStatus(id, status, isUnauthenticatedApproval, approverId, notificationId);
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"{_controllerName}: Record with id: {id} Approved Successfully at " + DateTime.UtcNow);
                    return true;
                }
                return false;
            }
            catch (Exception ex) { _logger.LogError($"{_controllerName} Approve method threw an exception for record with id: {id}, Message: {ex.Message}"); return false; }
        }
    }
}
