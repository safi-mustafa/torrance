using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.SOWService;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.SOW;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SOWController : CrudBaseController<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel, SOWDetailViewModel, SOWSearchViewModel>
    {
        private readonly ISOWService<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel> _SOWService;
        private readonly ILogger<SOWController> _logger;

        public SOWController(ISOWService<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel> SOWService, ILogger<SOWController> logger, IMapper mapper) : base(SOWService, logger, mapper, "SOW", "SOWs")
        {
            _SOWService = SOWService;
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
