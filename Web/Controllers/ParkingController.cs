using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices.MobileFileServices;
using ViewModels.AppSettings.MobileFiles.Parking;

namespace Web.Controllers
{
    [Authorize]
    
    public class ParkingController : CrudBaseController<ParkingModifyViewModel, ParkingModifyViewModel, ParkingDetailViewModel, ParkingDetailViewModel, ParkingSearchViewModel>
    {
        private readonly IMobileFileService<ParkingModifyViewModel, ParkingModifyViewModel, ParkingDetailViewModel> _parkingService;
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(IMobileFileService<ParkingModifyViewModel, ParkingModifyViewModel, ParkingDetailViewModel> parkingService, ILogger<ParkingController> logger, IMapper mapper) : base(parkingService, logger, mapper, "Parking", "Parking")
        {
            _parkingService = parkingService;
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
