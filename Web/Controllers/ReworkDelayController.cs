using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.ReworkService;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.ReworkDelay;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class ReworkDelayController : CrudBaseController<ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel, ReworkDelayDetailViewModel, ReworkDelaySearchViewModel>
    {
        private readonly IReworkDelayService<ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel> _ReworkDelayService;
        private readonly ILogger<ReworkDelayController> _logger;

        public ReworkDelayController(IReworkDelayService<ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel> ReworkDelayService, ILogger<ReworkDelayController> logger, IMapper mapper) : base(ReworkDelayService, logger, mapper, "ReworkDelay", "Rework Delay")
        {
            _ReworkDelayService = ReworkDelayService;
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
