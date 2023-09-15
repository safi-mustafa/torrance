using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.AppSettingServices.WeldMethodService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class WeldMethodController : CrudBaseController<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel, WeldMethodDetailViewModel, WeldMethodSearchViewModel>
    {
        private readonly IWeldMethodService<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel> _WeldMethodService;
        private readonly ILogger<WeldMethodController> _logger;

        public WeldMethodController(IWeldMethodService<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel> WeldMethodService, ILogger<WeldMethodController> logger, IMapper mapper) : base(WeldMethodService, logger, mapper, "WeldMethod", "Weld Method")
        {
            _WeldMethodService = WeldMethodService;
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
