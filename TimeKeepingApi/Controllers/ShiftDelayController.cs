using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.ShiftDelay;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftDelayController : CrudBaseBriefController<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel, ShiftDelayDetailViewModel, ShiftDelaySearchViewModel>
    {
        public ShiftDelayController(IShiftDelayService<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel> shiftDelayService) : base(shiftDelayService)
        {
        }
    }
}

