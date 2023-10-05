using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.OverrideLogs;
using Repositories.Services.OverrideLogServices.ReasonForRequestService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReasonForRequestController : CrudBaseBriefController<ReasonForRequestDetailViewModel, ReasonForRequestSearchViewModel>
    {
        public ReasonForRequestController(IReasonForRequestService<ReasonForRequestModifyViewModel, ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel> permitTypeService, ILogger<ReasonForRequestController> logger) : base(permitTypeService, logger, "ReasonForRequest")
        {
        }
    }
}

