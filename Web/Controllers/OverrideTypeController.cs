using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.OverrideTypeService;
using ViewModels.DataTable;
using ViewModels.OverrideLogs;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class OverrideTypeController : CrudBaseController<OverrideTypeModifyViewModel, OverrideTypeModifyViewModel, OverrideTypeDetailViewModel, OverrideTypeDetailViewModel, OverrideTypeSearchViewModel>
    {
        private readonly IOverrideTypeService<OverrideTypeModifyViewModel, OverrideTypeModifyViewModel, OverrideTypeDetailViewModel> _overrideTypeService;
        private readonly ILogger<OverrideTypeController> _logger;

        public OverrideTypeController(IOverrideTypeService<OverrideTypeModifyViewModel, OverrideTypeModifyViewModel, OverrideTypeDetailViewModel> overrideTypeService, ILogger<OverrideTypeController> logger, IMapper mapper) : base(overrideTypeService, logger, mapper, "OverrideType", "Override Types")
        {
            _overrideTypeService = overrideTypeService;
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
