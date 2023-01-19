using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.ReworkDelay;
using Repositories.Services.TimeOnToolServices.ReworkService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReworkDelayController : CrudBaseBriefController<ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel, ReworkDelayDetailViewModel, ReworkDelaySearchViewModel>
    {
        public ReworkDelayController(IReworkDelayService<ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel> reworkDelayService) : base(reworkDelayService)
        {
        }
    }
}

