using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.ReworkDelay;
using Repositories.Services.TimeOnToolServices.ReworkService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReworkDelayController : CrudBaseBriefController<ReworkDelayDetailViewModel, ReworkDelaySearchViewModel>
    {
        public ReworkDelayController(IReworkDelayService<ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel> reworkDelayService, ILogger<ReworkDelayController> logger) : base(reworkDelayService, logger, "ReworkDelay")
        {
        }
    }
}

