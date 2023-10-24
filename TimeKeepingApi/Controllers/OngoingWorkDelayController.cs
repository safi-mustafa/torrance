using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.OngoingWorkDelay;
using Repositories.Services.TimeOnToolServices.OngoingWorkDelayService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OngoingWorkDelayController : CrudBaseBriefController<OngoingWorkDelayDetailViewModel, OngoingWorkDelaySearchViewModel>
    {
        public OngoingWorkDelayController(IOngoingWorkDelayService<OngoingWorkDelayModifyViewModel, OngoingWorkDelayModifyViewModel, OngoingWorkDelayDetailViewModel> shiftDelayService, ILogger<OngoingWorkDelayController> logger) : base(shiftDelayService, logger, "OnGoingWorkDelay")
        {
        }
    }
}

