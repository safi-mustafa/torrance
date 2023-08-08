using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ViewModels.DataTable;
using ViewModels;
using Repositories;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class FCOReasonController : CrudBaseController<FCOReasonModifyViewModel, FCOReasonModifyViewModel, FCOReasonDetailViewModel, FCOReasonDetailViewModel, FCOReasonSearchViewModel>
    {
        private readonly IFCOReasonService<FCOReasonModifyViewModel, FCOReasonModifyViewModel, FCOReasonDetailViewModel> _FCOReasonService;
        private readonly ILogger<FCOReasonController> _logger;

        public FCOReasonController(IFCOReasonService<FCOReasonModifyViewModel, FCOReasonModifyViewModel, FCOReasonDetailViewModel> FCOReasonService, ILogger<FCOReasonController> logger, IMapper mapper) : base(FCOReasonService, logger, mapper, "FCOReason", "Rod Types")
        {
            _FCOReasonService = FCOReasonService;
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
