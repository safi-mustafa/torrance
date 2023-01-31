using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.CraftSkillService;
using ViewModels.DataTable;
using ViewModels.OverrideLogs;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CraftSkillController : CrudBaseController<CraftSkillModifyViewModel, CraftSkillModifyViewModel, CraftSkillDetailViewModel, CraftSkillDetailViewModel, CraftSkillSearchViewModel>
    {
        private readonly ICraftSkillService<CraftSkillModifyViewModel, CraftSkillModifyViewModel, CraftSkillDetailViewModel> _craftSkillService;
        private readonly ILogger<CraftSkillController> _logger;

        public CraftSkillController(ICraftSkillService<CraftSkillModifyViewModel, CraftSkillModifyViewModel, CraftSkillDetailViewModel> craftSkillService, ILogger<CraftSkillController> logger, IMapper mapper) : base(craftSkillService, logger, mapper, "CraftSkill", "CraftSkills")
        {
            _craftSkillService = craftSkillService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
