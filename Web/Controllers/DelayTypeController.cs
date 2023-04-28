using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.DelayTypeService;
using ViewModels.DataTable;
using ViewModels.TimeOnTools;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class DelayTypeController : CrudBaseController<DelayTypeModifyViewModel, DelayTypeModifyViewModel, DelayTypeDetailViewModel, DelayTypeDetailViewModel, DelayTypeSearchViewModel>
    {
        private readonly IDelayTypeService<DelayTypeModifyViewModel, DelayTypeModifyViewModel, DelayTypeDetailViewModel> _DelayTypeService;
        private readonly ILogger<DelayTypeController> _logger;

        public DelayTypeController(IDelayTypeService<DelayTypeModifyViewModel, DelayTypeModifyViewModel, DelayTypeDetailViewModel> DelayTypeService, ILogger<DelayTypeController> logger, IMapper mapper) : base(DelayTypeService, logger, mapper, "DelayType", "Delay Types")
        {
            _DelayTypeService = DelayTypeService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
