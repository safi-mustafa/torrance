using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.RodType;
using Repositories.Services.AppSettingServices.RodTypeService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RodTypeController : CrudBaseBriefController<RodTypeDetailViewModel, RodTypeSearchViewModel>
    {
        public RodTypeController(IRodTypeService<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel> rodTypeService) : base(rodTypeService)
        {
        }
    }
}

