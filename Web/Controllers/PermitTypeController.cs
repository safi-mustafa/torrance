using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.PermitType;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class PermitTypeController : CrudBaseController<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel, PermitTypeDetailViewModel, PermitTypeSearchViewModel>
    {
        private readonly IPermitTypeService<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel> _PermitTypeService;
        private readonly ILogger<PermitTypeController> _logger;

        public PermitTypeController(IPermitTypeService<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel> PermitTypeService, ILogger<PermitTypeController> logger, IMapper mapper) : base(PermitTypeService, logger, mapper, "PermitType", "Permit Type")
        {
            _PermitTypeService = PermitTypeService;
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
