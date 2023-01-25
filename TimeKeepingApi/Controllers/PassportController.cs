using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.AppSettings.MobileFiles.Passport;
using Repositories.Services.AppSettingServices.MobileFileServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PassportController : CrudBaseController<PassportModifyViewModel, PassportModifyViewModel, PassportDetailViewModel, PassportDetailViewModel, PassportSearchViewModel>
    {
        public PassportController(IMobileFileService<PassportModifyViewModel, PassportModifyViewModel, PassportDetailViewModel> passportService) : base(passportService)
        {
        }

        public override Task<IActionResult> Post([FromForm] PassportModifyViewModel model)
        {
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromForm] PassportModifyViewModel model)
        {
            return base.Put(model);
        }
    }
}

