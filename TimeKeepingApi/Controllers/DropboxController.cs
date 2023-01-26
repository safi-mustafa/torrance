using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.AppSettings.MobileFiles.Dropbox;
using Repositories.Services.AppSettingServices.MobileFileServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DropboxController : CrudBaseController<DropboxModifyViewModel, DropboxModifyViewModel, DropboxDetailViewModel, DropboxDetailViewModel, DropboxSearchViewModel>
    {
        public DropboxController(IMobileFileService<DropboxModifyViewModel, DropboxModifyViewModel, DropboxDetailViewModel> dropboxService) : base(dropboxService)
        {
        }

        public override Task<IActionResult> Post([FromForm] DropboxModifyViewModel model)
        {
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromForm] DropboxModifyViewModel model)
        {
            return base.Put(model);
        }
    }
}

