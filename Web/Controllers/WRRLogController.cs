﻿using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Packaging;
using Repositories.Services.AppSettingServices.WRRLogService;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.Common.Company;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver,Employee")]
    public class WRRLogController : ApproveBaseController<IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel>, WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel, WRRLogDetailViewModel, WRRLogSearchViewModel>
    {
        private readonly IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _WRRLogService;
        private readonly ILogger<WRRLogController> _logger;
        private readonly IUserInfoService _userInfo;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly string _loggedInUserRole;
        private readonly IBaseApprove _approveService;

        public WRRLogController(IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> WRRLogService, ILogger<WRRLogController> logger, IMapper mapper, IUserInfoService userInfo, UserManager<ToranceUser> userManager) : base(WRRLogService, logger, mapper, "WRRLog", "Welding Rod Record Log", userInfo)
        {
            _WRRLogService = WRRLogService;
            _logger = logger;
            _userInfo = userInfo;
            _userManager = userManager;
            _loggedInUserRole = _userInfo.LoggedInUserRole();
        }
        protected override WRRLogSearchViewModel SetDefaultFilters()
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
        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "<input type='checkbox' class='select-all-checkbox' onclick='selectAllCheckBoxChanged(this)'>",className="text-right exclude-from-export", data = ""},//
                new DataTableViewModel{title = "Status",data = "FormattedStatusForView",format="html",formatValue="status",exportColumn="FormattedStatus"},
                new DataTableViewModel{title = "Submitted",data = "FormattedCreatedOn", sortingColumn ="CreatedOn", orderable = true},
                new DataTableViewModel{title = "Requestor",data = "Employee.Name",sortingColumn="Employee.FullName", orderable=true},
                new DataTableViewModel{title = "Approver",data = "Approver.Name",sortingColumn="Approver.FullName", orderable=true},
                new DataTableViewModel{title = "Department",data = "Department.Name",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Unit",data = "Unit.Name", orderable=true,className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Company",data = "Company.Name",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Calibration Date",data = "FormattedCalibrationDate",sortingColumn="CalibrationDate", orderable = true},
                new DataTableViewModel{title = "Fume Control Used",data = "FumeControlUsed", orderable = true},
                new DataTableViewModel{title = "Rod Type",data = "RodType.Name",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Twr",data = "Twr", orderable = true},
                new DataTableViewModel{title = "Weld Method",data = "WeldMethod.Name",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Checkout",data = "FormattedRodCheckedOut",sortingColumn="RodCheckedOut", orderable = true},
                new DataTableViewModel{title = "Location",data = "Location.Name",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Rod Checked Out lbs",data = "RodCheckedOutLbs",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Rod Returned Waste lbs",data = "RodReturnedWasteLbs",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Returned",data = "FormattedDateRodReturned",sortingColumn="DateRodReturned", orderable = true},
                //new DataTableViewModel{title = "Email",data = "Email"},
                //new DataTableViewModel{title = "Status",data = "Status"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}
            });
            return dataColumns;

        }


        public async Task<IActionResult> ValidateWRRLogEmail(int id, string email)
        {
            return Json(await _WRRLogService.IsWRRLogEmailUnique(id, email));
        }

        public override async Task<ActionResult> Create()
        {
            var model = await GetCreateViewModel();
            return UpdateView(GetUpdateViewModel("Create", model));
        }
        [HttpPost]
        public override Task<ActionResult> Create(WRRLogModifyViewModel model)
        {
            if (User.IsInRole("Employee") || User.IsInRole("Approver"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Company.Id");
                ModelState.Remove("Company.Name");
            }
            return base.Create(model);
        }

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/WRRLog/_Index.cshtml", vm);
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>
            {
                new DataTableActionViewModel() { Action = "Detail", Title = "Detail", Href = $"/WRRLog/Detail/Id" },
                new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/WRRLog/Update/Id", ShowBasedOn = "CanUpdate" },
                new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/WRRLog/Delete/Id", ShowBasedOn = "CanDelete" }
            };
        }

        private async Task<WRRLogModifyViewModel> GetCreateViewModel()
        {
            var model = new WRRLogModifyViewModel();
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

        public async Task<IActionResult> DownloadExcel(WRRLogSearchViewModel searchModel)
        {
            var workBook = await _WRRLogService.DownloadExcel(searchModel);
            // Convert the workbook to a byte array
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                workBook.SaveAs(stream);
                fileBytes = stream.ToArray();
            }

            // Set the content type and file name for the Excel file
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "WeldingRodRecord Logs - Torrance.xlsx";

            // Return the Excel file as a FileStreamResult
            return File(new MemoryStream(fileBytes), contentType, fileName);
        }
    }
}
