using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.DelayTypeService;
using ViewModels.TimeOnTools;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DelayTypeController : CrudBaseBriefController<DelayTypeDetailViewModel, DelayTypeSearchViewModel>
    {
        public DelayTypeController(IDelayTypeService<DelayTypeModifyViewModel, DelayTypeModifyViewModel, DelayTypeDetailViewModel> permitTypeService, ILogger<DelayTypeController> logger) : base(permitTypeService, logger, "DelayType")
        {
        }
    }
}

