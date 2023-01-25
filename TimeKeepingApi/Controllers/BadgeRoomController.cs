using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.AppSettings.MobileFiles.BadgeRoom;
using Repositories.Services.AppSettingServices.MobileFileServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BadgeRoomController : CrudBaseController<BadgeRoomModifyViewModel, BadgeRoomModifyViewModel, BadgeRoomDetailViewModel, BadgeRoomDetailViewModel, BadgeRoomSearchViewModel>
    {
        public BadgeRoomController(IMobileFileService<BadgeRoomModifyViewModel, BadgeRoomModifyViewModel, BadgeRoomDetailViewModel> badgeRoomService) : base(badgeRoomService)
        {
        }

        public override Task<IActionResult> Post([FromForm] BadgeRoomModifyViewModel model)
        {
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromForm] BadgeRoomModifyViewModel model)
        {
            return base.Put(model);
        }
    }
}

