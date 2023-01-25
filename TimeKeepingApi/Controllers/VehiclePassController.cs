using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.AppSettings.MobileFiles.VehiclePass;
using Repositories.Services.AppSettingServices.MobileFileServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclePassController : CrudBaseController<VehiclePassModifyViewModel, VehiclePassModifyViewModel, VehiclePassDetailViewModel, VehiclePassDetailViewModel, VehiclePassSearchViewModel>
    {
        public VehiclePassController(IMobileFileService<VehiclePassModifyViewModel, VehiclePassModifyViewModel, VehiclePassDetailViewModel> vehiclePassService) : base(vehiclePassService)
        {
        }

        public override Task<IActionResult> Post([FromForm] VehiclePassModifyViewModel model)
        {
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromForm] VehiclePassModifyViewModel model)
        {
            return base.Put(model);
        }
    }
}

