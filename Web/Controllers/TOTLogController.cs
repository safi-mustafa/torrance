using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using ViewModels.DataTable;
using ViewModels.TomeOnTools.TOTLog;

namespace Web.Controllers
{
    [Authorize]
    public class TOTLogController : CrudBaseController<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogSearchViewModel>
    {
        private readonly ITOTLogService _TOTLogService;
        private readonly ILogger<TOTLogController> _logger;

        public TOTLogController(ITOTLogService TOTLogService, ILogger<TOTLogController> logger, IMapper mapper) : base(TOTLogService, logger, mapper, "TOTLog", "TOTLogs")
        {
            _TOTLogService = TOTLogService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Date",data = "FormattedDate"},
                new DataTableViewModel{title = "Twr",data = "Twr"},
                new DataTableViewModel{title = "Man Hours",data = "ManHours"},
                new DataTableViewModel{title = "Start Of Work",data = "FormattedStartOfWork"},
                new DataTableViewModel{title = "Man Power",data = "ManPower"},
                new DataTableViewModel{title = "Equipment No",data = "EquipmentNo"},
                new DataTableViewModel{title = "Hours Delayed",data = "HoursDelayed"},
                new DataTableViewModel{title = "Status",data = "Status"},
                //new DataTableViewModel{title = "Department",data = "Department.Name"},
                //new DataTableViewModel{title = "Unit",data = "Unit.Name"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            };
        }
    }
}
