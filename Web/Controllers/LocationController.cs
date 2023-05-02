using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.AppSettingServices.LocationService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Location;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class LocationController : CrudBaseController<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel, LocationDetailViewModel, LocationSearchViewModel>
    {
        private readonly ILocationService<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel> _LocationService;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocationService<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel> LocationService, ILogger<LocationController> logger, IMapper mapper) : base(LocationService, logger, mapper, "Location", "WRR Locations")
        {
            _LocationService = LocationService;
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
