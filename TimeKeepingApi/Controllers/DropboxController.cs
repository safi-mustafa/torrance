using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.AppSettings.MobileFiles.Dropbox;
using Repositories.Services.AppSettingServices.DropboxServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DropboxController : CrudBaseBriefController<DropboxDetailViewModel, DropboxSearchViewModel>
    {
        public DropboxController(IDropboxService<DropboxModifyViewModel, DropboxModifyViewModel, DropboxDetailViewModel> dropboxService) : base(dropboxService)
        {
        }

        public override Task<IActionResult> GetAll([FromQuery] DropboxSearchViewModel search)
        {
            search.ActiveStatus = Enums.ActiveStatus.Active;
            return base.GetAll(search);
        }
    }
}

