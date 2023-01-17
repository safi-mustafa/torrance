using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.ShiftService;
using ViewModels.DataTable;
using ViewModels.TomeOnTools.Shift;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class ShiftController : CrudBaseController<ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel, ShiftDetailViewModel, ShiftSearchViewModel>
    {
        private readonly IShiftService _ShiftService;
        private readonly ILogger<ShiftController> _logger;

        public ShiftController(IShiftService ShiftService, ILogger<ShiftController> logger, IMapper mapper) : base(ShiftService, logger, mapper, "Shift", "Shifts")
        {
            _ShiftService = ShiftService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
