using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.OverrideLogs;
using Repositories.Services.OverrideLogServices.ReasonForRequestService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReasonForRequestController : CrudBaseBriefController<ReasonForRequestModifyViewModel, ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel, ReasonForRequestDetailViewModel, ReasonForRequestSearchViewModel>
    {
        public ReasonForRequestController(IReasonForRequestService<ReasonForRequestModifyViewModel, ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

