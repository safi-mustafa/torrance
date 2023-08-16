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
    public class FCOReasonController : CrudBaseBriefController<FCOReasonDetailViewModel, FCOReasonSearchViewModel>
    {
        public FCOReasonController(IFCOReasonService<FCOReasonModifyViewModel, FCOReasonModifyViewModel, FCOReasonDetailViewModel> service) : base(service)
        {
        }
    }
}

