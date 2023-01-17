using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.WeldRodRecordServices.LocationService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Location;

namespace Web.Controllers
{
    [Authorize]
    public class LocationController : CrudBaseController<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel, LocationDetailViewModel, LocationSearchViewModel>
    {
        private readonly ILocationService _LocationService;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocationService LocationService, ILogger<LocationController> logger, IMapper mapper) : base(LocationService, logger, mapper, "Location", "Locations")
        {
            _LocationService = LocationService;
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
