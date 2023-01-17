using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.WeldRodRecordServices.RodTypeService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.RodType;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class RodTypeController : CrudBaseController<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel, RodTypeDetailViewModel, RodTypeSearchViewModel>
    {
        private readonly IRodTypeService _RodTypeService;
        private readonly ILogger<RodTypeController> _logger;

        public RodTypeController(IRodTypeService RodTypeService, ILogger<RodTypeController> logger, IMapper mapper) : base(RodTypeService, logger, mapper, "RodType", "RodTypes")
        {
            _RodTypeService = RodTypeService;
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
