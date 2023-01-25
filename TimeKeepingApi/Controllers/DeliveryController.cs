using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.AppSettings.MobileFiles.Delivery;
using Repositories.Services.AppSettingServices.MobileFileServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryController : CrudBaseController<DeliveryModifyViewModel, DeliveryModifyViewModel, DeliveryDetailViewModel, DeliveryDetailViewModel, DeliverySearchViewModel>
    {
        public DeliveryController(IMobileFileService<DeliveryModifyViewModel, DeliveryModifyViewModel, DeliveryDetailViewModel> deliveryService) : base(deliveryService)
        {
        }

        public override Task<IActionResult> Post([FromForm] DeliveryModifyViewModel model)
        {
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromForm] DeliveryModifyViewModel model)
        {
            return base.Put(model);
        }
    }
}

