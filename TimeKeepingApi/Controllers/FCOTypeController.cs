using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repositories;
using ViewModels;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FCOTypeController : CrudBaseBriefController<FCOTypeDetailViewModel, FCOTypeSearchViewModel>
    {
        public FCOTypeController(IFCOTypeService<FCOTypeModifyViewModel, FCOTypeModifyViewModel, FCOTypeDetailViewModel> service) : base(service)
        {
        }
    }
}

