using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.CraftSkillService;
using Repositories.Services.OverrideLogServices.LeadPlannerService;
using Repositories.Services.OverrideLogServices.OverrideTypeService;
using Repositories.Services.TimeOnToolServices.PermittingIssueService;
using ViewModels.DataTable;
using ViewModels.TomeOnTools.PermittingIssue;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PermittingIssueController : CrudBaseController<PermittingIssueModifyViewModel, PermittingIssueModifyViewModel, PermittingIssueDetailViewModel, PermittingIssueDetailViewModel, PermittingIssueSearchViewModel>
    {
        private readonly IPermittingIssueService<PermittingIssueModifyViewModel, PermittingIssueModifyViewModel, PermittingIssueDetailViewModel> _permittingIssueService;
        private readonly ILogger<PermittingIssueController> _logger;

        public PermittingIssueController(IPermittingIssueService<PermittingIssueModifyViewModel, PermittingIssueModifyViewModel, PermittingIssueDetailViewModel> permittingIssueService, ILogger<PermittingIssueController> logger, IMapper mapper) : base(permittingIssueService, logger, mapper, "PermittingIssue", "Permitting Issues")
        {
            _permittingIssueService = permittingIssueService;
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
