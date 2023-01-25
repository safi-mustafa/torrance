using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices.MobileFileServices;
using ViewModels.AppSettings.MobileFiles.VehiclePass;

namespace Web.Controllers
{
    [Authorize]
    
    public class VehiclePassController : CrudBaseController<VehiclePassModifyViewModel, VehiclePassModifyViewModel, VehiclePassDetailViewModel, VehiclePassDetailViewModel, VehiclePassSearchViewModel>
    {
        private readonly IMobileFileService<VehiclePassModifyViewModel, VehiclePassModifyViewModel, VehiclePassDetailViewModel> _vehiclePassService;
        private readonly ILogger<VehiclePassController> _logger;

        public VehiclePassController(IMobileFileService<VehiclePassModifyViewModel, VehiclePassModifyViewModel, VehiclePassDetailViewModel> vehiclePassService, ILogger<VehiclePassController> logger, IMapper mapper) : base(vehiclePassService, logger, mapper, "VehiclePass", "Vehicle Pass")
        {
            _vehiclePassService = vehiclePassService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Url",data = "Url"},
                new DataTableViewModel{title = "Status",data = "FormattedStatus"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
