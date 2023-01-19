using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Repositories.Services.CommonServices.UnitService;
using Select2;
using ViewModels.Common.Unit;
using ViewModels.CRUD;
using ViewModels.DataTable;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class UnitController : CrudBaseController<UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel, UnitDetailViewModel, UnitSearchViewModel>
    {
        private readonly IUnitService<UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel> _UnitService;
        private readonly ILogger<UnitController> _logger;

        public UnitController(IUnitService<UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel> UnitService, ILogger<UnitController> logger, IMapper mapper) : base(UnitService, logger, mapper, "Unit", "Units")
        {
            _UnitService = UnitService;
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
