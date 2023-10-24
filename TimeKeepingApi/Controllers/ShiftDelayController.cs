using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.ShiftDelay;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShiftDelayController : CrudBaseBriefController<ShiftDelayDetailViewModel, ShiftDelaySearchViewModel>
    {
        public ShiftDelayController(IShiftDelayService<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel> shiftDelayService, ILogger<ShiftDelayController> logger) : base(shiftDelayService, logger, "ShiftDelay")
        {
        }
    }
}

