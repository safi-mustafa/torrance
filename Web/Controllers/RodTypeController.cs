using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.AppSettingServices.RodTypeService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.RodType;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class RodTypeController : CrudBaseController<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel, RodTypeDetailViewModel, RodTypeSearchViewModel>
    {
        private readonly IRodTypeService<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel> _RodTypeService;
        private readonly ILogger<RodTypeController> _logger;

        public RodTypeController(IRodTypeService<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel> RodTypeService, ILogger<RodTypeController> logger, IMapper mapper) : base(RodTypeService, logger, mapper, "RodType", "Rod Type")
        {
            _RodTypeService = RodTypeService;
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
