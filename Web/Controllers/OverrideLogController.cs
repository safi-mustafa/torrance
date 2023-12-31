﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.OverrideLogs.ORLog;
using Repositories.Services.OverrideLogServices.ORLogService;
using Enums;
using Microsoft.AspNetCore.Identity;
using Models;
using ViewModels.Common.Company;
using ClosedXML.Excel;

namespace Web.Controllers
{
    [Authorize]
    public class OverrideLogController : ApproveBaseController<IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel>, ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel, ORLogDetailViewModel, ORLogSearchViewModel>
    {
        private readonly IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> _overrideLogService;
        private readonly ILogger<OverrideLogController> _logger;
        private readonly IUserInfoService _userInfo;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly string _loggedInUserRole;
        private readonly IBaseApprove _baseApprove;

        public OverrideLogController(IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> OverrideLogService, ILogger<OverrideLogController> logger, IMapper mapper, IUserInfoService userInfo, UserManager<ToranceUser> userManager) : base(OverrideLogService, logger, mapper, "OverrideLog", "Override Log", userInfo)
        {
            _overrideLogService = OverrideLogService;
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
                new DataTableViewModel{title = "<input type='checkbox' class='select-all-checkbox' onclick='selectAllCheckBoxChanged(this)'>",className="text-right exclude-from-export", data = ""},//
                new DataTableViewModel{title = "Status",data = "FormattedStatusForView",format="html",formatValue="status",exportColumn="FormattedStatus"},
                new DataTableViewModel{title = "Submitted",data = "FormattedCreatedOn", sortingColumn="CreatedOn", orderable=true},
                new DataTableViewModel{title = "Requestor",data = "Employee.Name",sortingColumn="Employee.FullName", orderable=true},
                new DataTableViewModel{title = "Approver",data = "Approver.Name",sortingColumn="Approver.FullName", orderable=true},
                new DataTableViewModel{title = "Department",data = "Department.Name",orderable=true},
                new DataTableViewModel{title = "Unit",data = "Unit.Name", orderable=true},
                new DataTableViewModel{title = "Shift",data = "Shift.Name", orderable=true},
                new DataTableViewModel{title = "Work Date",data = "FormattedDateOfWorkCompleted", sortingColumn="WorkCompletedDate", orderable=true},
                new DataTableViewModel{title = "Workscope",data = "WorkScope",orderable=true},
                new DataTableViewModel{title = "Override Reason",data = "Reason", orderable = true},
                new DataTableViewModel{title = "Company",data = "Company.Name", orderable = true},
                new DataTableViewModel{title = "PO Number",data = "PoNumber", orderable=true},
                new DataTableViewModel{title = "Employee Name(s)",data = "EmployeeNames", orderable=true},
                new DataTableViewModel{title = "Clipped Employee(s)",data = "ClippedEmployeesUrl",format="html",formatValue="link"},
                new DataTableViewModel{title = "ST Hours",data = "TotalSTHours", orderable=true},
                new DataTableViewModel{title = "OT Hours",data = "TotalOTHours", orderable=true},
                new DataTableViewModel{title = "DT Hours",data = "TotalDTHours", orderable=true},
                new DataTableViewModel{title = "Total Hours",data = "TotalHours", orderable=true},
                new DataTableViewModel{title = "Total Head Count",data = "TotalHeadCount", orderable=true},
                new DataTableViewModel{title = "Total Cost",data = "TotalCost", orderable=true, className="dt-currency"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}
            }); ;
            return dataColumns;
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            //var actions = new List<DataTableActionViewModel>();
            //if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")|| User.IsInRole("Administrator"))
            //{
            //    actions.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/OverrideLog/Update/Id" });
            //    actions.Add(new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/OverrideLog/Delete/Id" });
            //}
            result.ActionsList.Add(new DataTableActionViewModel() { Action = "Detail", Title = "Detail", Href = $"/OverrideLog/Detail/Id" });
            result.ActionsList.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/OverrideLog/Update/Id", ShowBasedOn = "CanUpdate" });
            result.ActionsList.Add(new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/OverrideLog/Delete/Id", ShowBasedOn = "CanDelete" });


        }
        protected override ORLogSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.StatusNot.Add(Status.Pending);
            return filters;
        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            vm.IsExcelDownloadAjaxBased = true;
            return base.OverrideCrudListVM(vm);
        }
        public override async Task<ActionResult> Create()
        {
            var model = await GetCreateViewModel();
            return UpdateView(GetUpdateViewModel("Create", model));
        }
        public override Task<ActionResult> Create(ORLogModifyViewModel model)
        {
            if (User.IsInRole("Employee") || User.IsInRole("Approver"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Company.Id");
                ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Create(model);
        }

        public override Task<ActionResult> Update(ORLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
                ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Update(model);
        }

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/OverrideLog/_Index.cshtml", vm);
        }

        public async Task<IActionResult> DownloadExcel(ORLogSearchViewModel searchModel)
        {
            var workBook = await _overrideLogService.DownloadExcel(searchModel);
            // Convert the workbook to a byte array
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                workBook.SaveAs(stream);
                fileBytes = stream.ToArray();
            }

            // Set the content type and file name for the Excel file
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "override-logs.xlsx";

            // Return the Excel file as a FileStreamResult
            return File(new MemoryStream(fileBytes), contentType, fileName);
        }
        public IActionResult _CostRow(ORLogCostViewModel model, int rowNumber)
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

        public async Task MergeCostsForCraft()
        {
            await _overrideLogService.MergeCostsForCraft();
        }

        public async Task CalculateTotals()
        {
            await _overrideLogService.CalculateTotalCostAndHours();
            RedirectToAction("Index");
        }
    }

}
