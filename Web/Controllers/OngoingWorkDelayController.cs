using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.OngoingWorkDelayService;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.OngoingWorkDelay;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class OngoingWorkDelayController : CrudBaseController<OngoingWorkDelayModifyViewModel, OngoingWorkDelayModifyViewModel, OngoingWorkDelayDetailViewModel, OngoingWorkDelayDetailViewModel, OngoingWorkDelaySearchViewModel>
    {
        private readonly IOngoingWorkDelayService<OngoingWorkDelayModifyViewModel, OngoingWorkDelayModifyViewModel, OngoingWorkDelayDetailViewModel> _OngoingWorkDelayService;
        private readonly ILogger<OngoingWorkDelayController> _logger;

        public OngoingWorkDelayController(IOngoingWorkDelayService<OngoingWorkDelayModifyViewModel, OngoingWorkDelayModifyViewModel, OngoingWorkDelayDetailViewModel> OngoingWorkDelayService, ILogger<OngoingWorkDelayController> logger, IMapper mapper) : base(OngoingWorkDelayService, logger, mapper, "OngoingWorkDelay", "Ongoing Work Delays")
        {
            _OngoingWorkDelayService = OngoingWorkDelayService;
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
