﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.OverrideLogs;
using ViewModels.WeldingRodRecord.WRRLog;
using Repositories.Services.OverrideLogServices.ORLogService;
using ViewModels.TimeOnTools.TOTLog;
using Enums;
using Microsoft.AspNetCore.Identity;
using Models;
using ViewModels.Common.Company;

namespace Web.Controllers
{
    [Authorize]
    public class OverrideLogController : ApproveBaseController<IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel>, ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel, ORLogDetailViewModel, ORLogSearchViewModel>
    {
        private readonly IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> _OverrideLogService;
        private readonly ILogger<OverrideLogController> _logger;
        private readonly IUserInfoService _userInfo;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly string _loggedInUserRole;
        private readonly IBaseApprove _baseApprove;

        public OverrideLogController(IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> OverrideLogService, ILogger<OverrideLogController> logger, IMapper mapper, IUserInfoService userInfo,UserManager<ToranceUser> userManager) : base(OverrideLogService, logger, mapper, "OverrideLog", "Override Log", !(userInfo.LoggedInUserRoles().Contains("Administrator") || userInfo.LoggedInUserRoles().Contains("SuperAdmin") || userInfo.LoggedInUserRoles().Contains("Employee")))
        {
            _OverrideLogService = OverrideLogService;
            _logger = logger;
            _userInfo = userInfo;
            _userManager = userManager;
            _loggedInUserRole = _userInfo.LoggedInUserRole();
        }

        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "<input type='checkbox' class='select-all-checkbox' onclick='selectAllCheckBoxChanged(this)'>",className="text-right exclude-form-export", data = ""},//
                new DataTableViewModel{title = "Status",data = "FormattedStatus",format="html",formatValue="status",exportColumn="FormattedStatus"},
                new DataTableViewModel{title = "Submitted",data = "FormattedCreatedOn", sortingColumn="CreatedOn", orderable=true},
                new DataTableViewModel{title = "Requester",data = "Employee.Name", orderable=true},
                new DataTableViewModel{title = "Approver",data = "Approver.Name", orderable=true},
                new DataTableViewModel{title = "Shift",data = "Shift.Name", orderable=true},
                new DataTableViewModel{title = "Unit",data = "Unit.Name", orderable=true},
                //new DataTableViewModel{title = "Reason",data = "ReasonForRequest.Name", orderable=true},
                new DataTableViewModel{title = "Total Hours",data = "TotalHours", orderable=true},
                new DataTableViewModel{title = "Total Head Count",data = "TotalHeadCount", orderable=true},
                new DataTableViewModel{title = "Total Cost",data = "TotalCost", orderable=true, className="dt-currency"},
                new DataTableViewModel{title = "PO Number",data = "PoNumber", orderable=true},
                new DataTableViewModel{title = "Work Date",data = "FormattedDateOfWorkCompleted", sortingColumn="DateOfWorkCompleted", orderable=true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            }); ;
            return dataColumns;
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            var actions = new List<DataTableActionViewModel>();
            //if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")|| User.IsInRole("Administrator"))
            //{
            //    actions.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/OverrideLog/Update/Id" });
            //    actions.Add(new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/OverrideLog/Delete/Id" });
            //}
            actions.Add(new DataTableActionViewModel() { Action = "Detail", Title = "Detail", Href = $"/OverrideLog/Detail/Id" });
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole == RolesCatalog.CompanyManager.ToString())
            {
                result.ActionsList.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/OverrideLog/Update/Id", HideBasedOn = "IsEditRestricted" });
            }
            result.ActionsList = actions;
        }
        protected override ORLogSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.StatusNot = Enums.Status.Pending;
            return filters;
        }
        public override async Task<ActionResult> Create()
        {
            var model = await GetCreateViewModel();
            return UpdateView(GetUpdateViewModel("Create", model));
        }
        public override Task<ActionResult> Create(ORLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Requester.Id");
                ModelState.Remove("Requester.Name");
                ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Create(model);
        }

        public override Task<ActionResult> Update(ORLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Requester.Id");
                ModelState.Remove("Requester.Name");
                ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Update(model);
        }

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/OverrideLog/_Index.cshtml", vm);
        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            var html = "";
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole == RolesCatalog.CompanyManager.ToString())
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

        public IActionResult _CostRow(ORLogCostViewModel model, int rowNumber)//
        {
            ViewData["RowNumber"] = rowNumber;
            return PartialView("_CostRow", model);
        }

        private async Task<ORLogModifyViewModel> GetCreateViewModel()
        {
            var model = new ORLogModifyViewModel();
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, RolesCatalog.Employee.ToString()))
            {
                model.Company = new CompanyBriefViewModel();
                var companyIdClaim = User.Claims.Where(c => c.Type == "CompanyId").Select(x => x.Value).FirstOrDefault();
                if (companyIdClaim != null)
                {
                    model.Company.Id = int.Parse(companyIdClaim.ToString());
                }
                var companyNameClaim = User.Claims.Where(c => c.Type == "CompanyName").Select(x => x.Value).FirstOrDefault();
                if (companyNameClaim != null)
                {
                    model.Company.Name = companyNameClaim.ToString();
                }
            }
            return model;
        }
    }

}
