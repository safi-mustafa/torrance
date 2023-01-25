using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.AppSettings.MobileFiles.Parking;
using Repositories.Services.AppSettingServices.MobileFileServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParkingController : CrudBaseController<ParkingModifyViewModel, ParkingModifyViewModel, ParkingDetailViewModel, ParkingDetailViewModel, ParkingSearchViewModel>
    {
        public ParkingController(IMobileFileService<ParkingModifyViewModel, ParkingModifyViewModel, ParkingDetailViewModel> parkingService) : base(parkingService)
        {
        }

        public override Task<IActionResult> Post([FromForm] ParkingModifyViewModel model)
        {
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromForm] ParkingModifyViewModel model)
        {
            return base.Put(model);
        }
    }
}

