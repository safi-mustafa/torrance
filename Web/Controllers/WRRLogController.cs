using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.WeldRodRecordServices.WRRLogService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Web.Controllers
{
    [Authorize]
    public class WRRLogController : CrudBaseController<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel, WRRLogDetailViewModel, WRRLogSearchViewModel>
    {
        private readonly IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _WRRLogService;
        private readonly ILogger<WRRLogController> _logger;

        public WRRLogController(IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> WRRLogService, ILogger<WRRLogController> logger, IMapper mapper) : base(WRRLogService, logger, mapper, "WRRLog", "Welding Rod Record Logs")
        {
            _WRRLogService = WRRLogService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Date Rod Returned",data = "FormattedDateRodReturned"},
                new DataTableViewModel{title = "Calibration Date",data = "FormattedCalibrationDate"},
                new DataTableViewModel{title = "Fume Control Used",data = "FumeControlUsed"},
                new DataTableViewModel{title = "Twr",data = "Twr"},
                new DataTableViewModel{title = "Email",data = "Email"},
                new DataTableViewModel{title = "Rod Checked Out",data = "FormattedRodCheckedOut"},
                new DataTableViewModel{title = "Rod Checked Out lbs",data = "RodCheckedOutLbs"},
                new DataTableViewModel{title = "Rod Returned Waste lbs",data = "RodReturnedWasteLbs"},
                //new DataTableViewModel{title = "Department",data = "Department.Name"},
                //new DataTableViewModel{title = "Employee",data = "Employee.Name"},
                //new DataTableViewModel{title = "Unit",data = "Unit.Name"},
                //new DataTableViewModel{title = "RodType",data = "RodType.Name"},
                //new DataTableViewModel{title = "WeldMethod",data = "WeldMethod.Name"},
                //new DataTableViewModel{title = "Location",data = "Location.Name"},
                //new DataTableViewModel{title = "Name",data = "Name"},


                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            };
        }
        public async Task<IActionResult> ValidateWRRLogEmail(int id, string email)
        {
            return Json(await _WRRLogService.IsWRRLogEmailUnique(id, email));
        }
        [HttpPost]
        public override Task<ActionResult> Create(WRRLogModifyViewModel model)
        {
            if (!User.IsInRole("Admin")) {
                ModelState.Remove("Employee.Id");
            
            }
            return base.Create(model);
        }
    }
}
