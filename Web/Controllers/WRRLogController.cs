using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Repositories.Services.AppSettingServices.WRRLogService;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Approver")]
    public class WRRLogController : ApproveBaseController<IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel>, WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel, WRRLogDetailViewModel, WRRLogSearchViewModel>
    {
        private readonly IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _WRRLogService;
        private readonly ILogger<WRRLogController> _logger;
        private readonly IBaseApprove _approveService;

        public WRRLogController(IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> WRRLogService, ILogger<WRRLogController> logger, IMapper mapper, IUserInfoService userInfo) : base(WRRLogService, logger, mapper, "WRRLog", "Welding Rod Record Logs", !(userInfo.LoggedInUserRoles().Contains("Admin") || userInfo.LoggedInUserRoles().Contains("SuperAdmin")))
        {
            _WRRLogService = WRRLogService;
            _logger = logger;
        }
        protected override WRRLogSearchViewModel SetDefaultFilters()
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
                new DataTableViewModel{title = "Status",data = "FormattedStatus",format="html",formatValue="status"},
                  new DataTableViewModel{title = "Submitted",data = "FormattedCreatedOn", sortingColumn ="CreatedOn", orderable = true},
                new DataTableViewModel{title = "Requester",data = "Employee.Name", orderable=true},
                new DataTableViewModel{title = "Approver",data = "Approver.Name", orderable=true},
                new DataTableViewModel{title = "Date Rod Returned",data = "FormattedDateRodReturned"},
                new DataTableViewModel{title = "Calibration Date",data = "FormattedCalibrationDate"},
                new DataTableViewModel{title = "Fume Control Used",data = "FumeControlUsed"},
                new DataTableViewModel{title = "Twr",data = "Twr"},
                new DataTableViewModel{title = "Email",data = "Email"},
                //new DataTableViewModel{title = "Status",data = "Status"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            });
            return dataColumns;

        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            vm.DataTableHeaderHtml = @"
                    <div class=""p-2 row"">
                        <span class=""badge Approved m-1""> </span>
                        <span class=""stat-name"">Approved</span>
                    </div>
                    <div class=""m-2 row"">
                        <span class=""badge Rejected m-1""> </span>
                        <span class=""stat-name"">Rejected</span>
                    </div>";
            vm.IsResponsiveDatatable = false;
            return vm;
        }
        public async Task<IActionResult> ValidateWRRLogEmail(int id, string email)
        {
            return Json(await _WRRLogService.IsWRRLogEmailUnique(id, email));
        }
        [HttpPost]
        public override Task<ActionResult> Create(WRRLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            return base.Create(model);
        }
        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/WRRLog/_Index.cshtml", vm);
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/WRRLog/Detail/Id"},

            };
        }
    }
}
