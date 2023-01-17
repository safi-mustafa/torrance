using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using ViewModels.DataTable;
using ViewModels.TomeOnTools.PermitType;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class PermitTypeController : CrudBaseController<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel, PermitTypeDetailViewModel, PermitTypeSearchViewModel>
    {
        private readonly IPermitTypeService _PermitTypeService;
        private readonly ILogger<PermitTypeController> _logger;

        public PermitTypeController(IPermitTypeService PermitTypeService, ILogger<PermitTypeController> logger, IMapper mapper) : base(PermitTypeService, logger, mapper, "PermitType", "PermitTypes")
        {
            _PermitTypeService = PermitTypeService;
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
