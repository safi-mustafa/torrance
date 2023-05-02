using BainBridgeApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.CraftSkillService;
using ViewModels.OverrideLogs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CraftSkillController : CrudBaseBriefController<CraftSkillDetailViewModel, CraftSkillSearchViewModel>
    {
        public CraftSkillController(ICraftSkillService<CraftSkillModifyViewModel, CraftSkillModifyViewModel, CraftSkillDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

