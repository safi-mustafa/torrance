using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Repositories.Services.OverrideLogServices.CraftSkillService;
using ViewModels.DataTable;
using ViewModels.OverrideLogs;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class CraftSkillController : CrudBaseController<CraftSkillModifyViewModel, CraftSkillModifyViewModel, CraftSkillDetailViewModel, CraftSkillDetailViewModel, CraftSkillSearchViewModel>
    {
        private readonly ICraftSkillService<CraftSkillModifyViewModel, CraftSkillModifyViewModel, CraftSkillDetailViewModel> _craftSkillService;
        private readonly ILogger<CraftSkillController> _logger;

        public CraftSkillController(ICraftSkillService<CraftSkillModifyViewModel, CraftSkillModifyViewModel, CraftSkillDetailViewModel> craftSkillService, ILogger<CraftSkillController> logger, IMapper mapper) : base(craftSkillService, logger, mapper, "CraftSkill", "Craft Skill")
        {
            _craftSkillService = craftSkillService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "ST Rate",data = "STRate", className="dt-currency",orderable=true},
                new DataTableViewModel{title = "OT Rate",data = "OTRate", className="dt-currency", orderable = true},
                new DataTableViewModel{title = "DT Rate",data = "DTRate", className="dt-currency", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }

      
    }
}
