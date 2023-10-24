//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Repositories.Services.OverrideLogServices.CraftRateService;
//using ViewModels.DataTable;
//using ViewModels.OverrideLogs;

//namespace Web.Controllers
//{
//    [Authorize(Roles = "SuperAdmin")]
//    public class CraftRateController : CrudBaseController<CraftRateModifyViewModel, CraftRateModifyViewModel, CraftRateDetailViewModel, CraftRateDetailViewModel, CraftRateSearchViewModel>
//    {
//        //private readonly ICraftRateService<CraftRateModifyViewModel, CraftRateModifyViewModel, CraftRateDetailViewModel> _craftRateService;
//        private readonly ILogger<CraftRateController> _logger;

//        public CraftRateController(ICraftRateService<CraftRateModifyViewModel, CraftRateModifyViewModel, CraftRateDetailViewModel> craftRateService, ILogger<CraftRateController> logger, IMapper mapper) : base(craftRateService, logger, mapper, "CraftRate", "Craft Rate")
//        {
//            _craftRateService = craftRateService;
//            _logger = logger;
//        }

//        public override List<DataTableViewModel> GetColumns()
//        {
//            return new List<DataTableViewModel>()
//            {
//                new DataTableViewModel{title = "Rate",data = "Rate",orderable=true},
//                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

//            };
//        }
//    }
//}
