using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.Shift;
using Repositories.Services.TimeOnToolServices.ShiftService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : CrudBaseBriefController<ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel, ShiftDetailViewModel, ShiftSearchViewModel>
    {
        public ShiftController(IShiftService<ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel> shiftService) : base(shiftService)
        {
        }
    }
}

