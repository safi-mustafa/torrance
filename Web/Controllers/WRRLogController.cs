using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Repositories.Services.WeldRodRecordServices.WRRLogService;
using Repositories.Shared.Interfaces;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Web.Controllers
{
    [Authorize]
    public class WRRLogController : ApproveBaseController<IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel>, WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel, WRRLogDetailViewModel, WRRLogSearchViewModel>
    {
        private readonly IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _WRRLogService;
        private readonly ILogger<WRRLogController> _logger;
        private readonly IBaseApprove _approveService;

        public WRRLogController(IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> WRRLogService, ILogger<WRRLogController> logger, IMapper mapper) : base(WRRLogService, logger, mapper, "WRRLog", "Welding Rod Record Logs")
        {
            _WRRLogService = WRRLogService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            if (!User.IsInRole("Employee"))
            {
                dataColumns.Add(new DataTableViewModel { title = "<input type='checkbox' id='master-checkbox'>", data = "Id", format = "html", formatValue = "checkbox", className = "exclude-form-export" });
            }
            dataColumns.AddRange( new List<DataTableViewModel>()
            {

                new DataTableViewModel{title = "Date Rod Returned",data = "FormattedDateRodReturned"},
                new DataTableViewModel{title = "Calibration Date",data = "FormattedCalibrationDate"},
                new DataTableViewModel{title = "Fume Control Used",data = "FumeControlUsed"},
                new DataTableViewModel{title = "Twr",data = "Twr"},
                new DataTableViewModel{title = "Email",data = "Email"},
                new DataTableViewModel{title = "Status",data = "Status"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            });
            return dataColumns;
            
        }
        public async Task<IActionResult> ValidateWRRLogEmail(int id, string email)
        {
            return Json(await _WRRLogService.IsWRRLogEmailUnique(id, email));
        }
        [HttpPost]
        public override Task<ActionResult> Create(WRRLogModifyViewModel model)
        {
            if (User.IsInRole("Employee")) {
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

            if (!User.IsInRole("Employee"))
            {
                result.ActionsList.AddRange(new List<DataTableActionViewModel>()
                    {
                        new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/WRRLog/Update/Id" },
                        new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/WRRLog/Delete/Id" }
                    }
                );
            }
        }
    }
}
