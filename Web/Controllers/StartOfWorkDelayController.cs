using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.StartOfWorkDelayService;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.StartOfWorkDelay;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class StartOfWorkDelayController : CrudBaseController<StartOfWorkDelayModifyViewModel, StartOfWorkDelayModifyViewModel, StartOfWorkDelayDetailViewModel, StartOfWorkDelayDetailViewModel, StartOfWorkDelaySearchViewModel>
    {
        private readonly IStartOfWorkDelayService<StartOfWorkDelayModifyViewModel, StartOfWorkDelayModifyViewModel, StartOfWorkDelayDetailViewModel> _StartOfWorkDelayService;
        private readonly ILogger<StartOfWorkDelayController> _logger;

        public StartOfWorkDelayController(IStartOfWorkDelayService<StartOfWorkDelayModifyViewModel, StartOfWorkDelayModifyViewModel, StartOfWorkDelayDetailViewModel> StartOfWorkDelayService, ILogger<StartOfWorkDelayController> logger, IMapper mapper) : base(StartOfWorkDelayService, logger, mapper, "StartOfWorkDelay", "Start Of Work Delay")
        {
            _StartOfWorkDelayService = StartOfWorkDelayService;
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
