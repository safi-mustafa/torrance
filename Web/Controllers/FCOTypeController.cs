using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ViewModels.DataTable;
using ViewModels;
using Repositories;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class FCOTypeController : CrudBaseController<FCOTypeModifyViewModel, FCOTypeModifyViewModel, FCOTypeDetailViewModel, FCOTypeDetailViewModel, FCOTypeSearchViewModel>
    {
        private readonly IFCOTypeService<FCOTypeModifyViewModel, FCOTypeModifyViewModel, FCOTypeDetailViewModel> _FCOTypeService;
        private readonly ILogger<FCOTypeController> _logger;

        public FCOTypeController(IFCOTypeService<FCOTypeModifyViewModel, FCOTypeModifyViewModel, FCOTypeDetailViewModel> FCOTypeService, ILogger<FCOTypeController> logger, IMapper mapper) : base(FCOTypeService, logger, mapper, "FCOType", "FCO Types")
        {
            _FCOTypeService = FCOTypeService;
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
