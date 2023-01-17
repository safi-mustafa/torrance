using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;
using ViewModels.DataTable;
using ViewModels.TomeOnTools.ShiftDelay;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class ShiftDelayController : CrudBaseController<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel, ShiftDelayDetailViewModel, ShiftDelaySearchViewModel>
    {
        private readonly IShiftDelayService _ShiftDelayService;
        private readonly ILogger<ShiftDelayController> _logger;

        public ShiftDelayController(IShiftDelayService ShiftDelayService, ILogger<ShiftDelayController> logger, IMapper mapper) : base(ShiftDelayService, logger, mapper, "ShiftDelay", "ShiftDelays")
        {
            _ShiftDelayService = ShiftDelayService;
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
