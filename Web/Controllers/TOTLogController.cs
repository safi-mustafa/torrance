using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Pagination;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using Select2;
using Select2.Model;
using ViewModels;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "SuperAdmin,Approver,Employee")]
    public class TOTLogController : ApproveBaseController<ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel>, TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogSearchViewModel>
    {
        private readonly ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> _TOTLogService;
        private readonly ILogger<TOTLogController> _logger;
        private readonly string _loggedInUserRole;
        private readonly IUserInfoService _userInfo;
        private readonly IBaseApprove _baseApprove;

        public TOTLogController(ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> TOTLogService, ILogger<TOTLogController> logger, IMapper mapper, IUserInfoService userInfo) : base(TOTLogService, logger, mapper, "TOTLog", "Time On Tool Logs", !(userInfo.LoggedInUserRoles().Contains("Admin") || userInfo.LoggedInUserRoles().Contains("SuperAdmin") || userInfo.LoggedInUserRoles().Contains("Employee")))
        {
            _TOTLogService = TOTLogService;
            _logger = logger;
            _userInfo = userInfo;
            _loggedInUserRole = _userInfo.LoggedInUserRole();
        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            var html = "";
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole== RolesCatalog.CompanyManager.ToString())
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
                    </div>";
            vm.DataTableHeaderHtml = html;
            vm.IsResponsiveDatatable = false;
            return vm;
        }

        protected override TOTLogSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.StatusNot = Enums.Status.Pending;
            return filters;
        }
        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "<input type='checkbox' class='select-all-checkbox' onclick='selectAllCheckBoxChanged(this)'>",className="text-right exclude-form-export", data = ""},//
                new DataTableViewModel{title = "Status",data = "FormattedStatus",format="html",formatValue="status",exportColumn="FormattedStatus"},
                new DataTableViewModel{title = "Submitted",data = "FormattedCreatedOn", sortingColumn ="CreatedOn", orderable = true},
                new DataTableViewModel{title = "Requester",data = "Employee.Name", orderable=true},
                new DataTableViewModel{title = "Approver",data = "Approver.Name", orderable=true},
                new DataTableViewModel{title = "Shift",data = "Shift.Name", orderable=true},
                new DataTableViewModel{title = "Unit",data = "Unit.Name", orderable=true},
                new DataTableViewModel{title = "TWR",data = "Twr", orderable=true},
                new DataTableViewModel{title = "Equip.",data = "EquipmentNo", orderable=true},
                //new DataTableViewModel{title = "Delay Type",data = "DelayType.Name", orderable=true},
                new DataTableViewModel{title = "Reason",data = "ReasonForRequest.Name", orderable=true},
                new DataTableViewModel{title = "Permit",data = "PermitType.Name", orderable=true},
                new DataTableViewModel{title = "Date",data = "FormattedStartOfWork", sortingColumn="StartOfWork", orderable=true},
                new DataTableViewModel{title = "HC",data = "ManPowerAffected", orderable=true},
                new DataTableViewModel{title = "MH",data = "ManHours", orderable=true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            });
            return dataColumns;
        }

        public override Task<ActionResult> Create(TOTLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            return base.Create(model);
        }

        public override Task<ActionResult> Update(int id)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            return base.Update(id);
        }

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/TOTLog/_Index.cshtml", vm);
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/TOTLog/Detail/Id"},
            };
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole == RolesCatalog.CompanyManager.ToString())
            {
                result.ActionsList.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/TOTLog/Update/Id", HideBasedOn = "IsEditRestricted" });
            }
        }

        public async Task<JsonResult> GetTWRNumericValues(string prefix, int pageSize, int pageNumber, string customParams)
        {
            try
            {
                var svm = SetSelect2CustomParams(customParams);
                svm.PerPage = pageSize;
                svm.CalculateTotal = true;
                svm.CurrentPage = pageNumber;
                svm.DisablePagination = true;
                svm.Search = new DataTableSearchViewModel() { value = prefix };
                var response = await _TOTLogService.GetTWRNumericValues<Select2ViewModel>(svm);
                PaginatedResultModel<Select2ViewModel> items = new();
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<Select2ViewModel>>;
                    items = parsedResponse?.ReturnModel ?? new();
                }
                if (svm.DisablePagination)
                {
                    pageSize = items.Items.Count;
                }
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, items.Items.ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"TOTLog Select2 method threw an exception, Message: {ex.Message}");
                return null;
            }


        }

        public async Task<JsonResult> GetTWRAlphabeticValues(string prefix, int pageSize, int pageNumber, string customParams)
        {
            try
            {
                var svm = SetSelect2CustomParams(customParams);
                svm.PerPage = pageSize;
                svm.CalculateTotal = true;
                svm.CurrentPage = pageNumber;
                svm.DisablePagination = true;
                svm.Search = new DataTableSearchViewModel() { value = prefix };
                var response = await _TOTLogService.GetTWRAphabeticValues<Select2ViewModel>(svm);
                PaginatedResultModel<Select2ViewModel> items = new();
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<Select2ViewModel>>;
                    items = parsedResponse?.ReturnModel ?? new();
                }
                if (svm.DisablePagination)
                {
                    pageSize = items.Items.Count;
                }
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, items.Items.ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"TOTLog Select2 method threw an exception, Message: {ex.Message}");
                return null;
            }


        }


    }

}
