using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.SOWService;
using ViewModels.DataTable;
using ViewModels.TomeOnTools.SOW;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class SOWController : CrudBaseController<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel, SOWDetailViewModel, SOWSearchViewModel>
    {
        private readonly ISOWService _SOWService;
        private readonly ILogger<SOWController> _logger;

        public SOWController(ISOWService SOWService, ILogger<SOWController> logger, IMapper mapper) : base(SOWService, logger, mapper, "SOW", "SOWs")
        {
            _SOWService = SOWService;
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
