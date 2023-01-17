using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.WeldRodRecordServices.WeldMethodService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class WeldMethodController : CrudBaseController<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel, WeldMethodDetailViewModel, WeldMethodSearchViewModel>
    {
        private readonly IWeldMethodService _WeldMethodService;
        private readonly ILogger<WeldMethodController> _logger;

        public WeldMethodController(IWeldMethodService WeldMethodService, ILogger<WeldMethodController> logger, IMapper mapper) : base(WeldMethodService, logger, mapper, "WeldMethod", "WeldMethods")
        {
            _WeldMethodService = WeldMethodService;
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
