using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.LeadPlannerService;
using ViewModels.OverrideLogs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeadPlannerController : CrudBaseBriefController<LeadPlannerModifyViewModel, LeadPlannerModifyViewModel, LeadPlannerDetailViewModel, LeadPlannerDetailViewModel, LeadPlannerSearchViewModel>
    {
        public LeadPlannerController(ILeadPlannerService<LeadPlannerModifyViewModel, LeadPlannerModifyViewModel, LeadPlannerDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

