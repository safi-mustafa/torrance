using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.StartOfWorkDelay;
using Repositories.Services.TimeOnToolServices.StartOfWorkDelayService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StartOfWorkDelayController : CrudBaseBriefController<StartOfWorkDelayDetailViewModel, StartOfWorkDelaySearchViewModel>
    {
        public StartOfWorkDelayController(IStartOfWorkDelayService<StartOfWorkDelayModifyViewModel, StartOfWorkDelayModifyViewModel, StartOfWorkDelayDetailViewModel> shiftDelayService) : base(shiftDelayService)
        {
        }
    }
}

