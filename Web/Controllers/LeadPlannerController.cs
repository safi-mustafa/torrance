using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.LeadPlannerService;
using ViewModels.DataTable;
using ViewModels.OverrideLogs;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class LeadPlannerController : CrudBaseController<LeadPlannerModifyViewModel, LeadPlannerModifyViewModel, LeadPlannerDetailViewModel, LeadPlannerDetailViewModel, LeadPlannerSearchViewModel>
    {
        private readonly ILeadPlannerService<LeadPlannerModifyViewModel, LeadPlannerModifyViewModel, LeadPlannerDetailViewModel> _leadPlannerService;
        private readonly ILogger<LeadPlannerController> _logger;

        public LeadPlannerController(ILeadPlannerService<LeadPlannerModifyViewModel, LeadPlannerModifyViewModel, LeadPlannerDetailViewModel> leadPlannerService, ILogger<LeadPlannerController> logger, IMapper mapper) : base(leadPlannerService, logger, mapper, "LeadPlanner", "Lead Planners")
        {
            _leadPlannerService = leadPlannerService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }
    }
}
