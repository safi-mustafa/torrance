using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.ShiftService;
using ViewModels.AppSettings.Map;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.Shift;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class ShiftController : CrudBaseController<ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel, ShiftDetailViewModel, ShiftSearchViewModel>
    {
        private readonly IShiftService<ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel> _ShiftService;
        private readonly ILogger<ShiftController> _logger;

        public ShiftController(IShiftService<ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel> ShiftService, ILogger<ShiftController> logger, IMapper mapper) : base(ShiftService, logger, mapper, "Shift", "Shifts")
        {
            _ShiftService = ShiftService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }
    }
}
