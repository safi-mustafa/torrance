using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.Location;
using Repositories.Services.AppSettingServices.LocationService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : CrudBaseBriefController<LocationDetailViewModel, LocationSearchViewModel>
    {
        public LocationController(ILocationService<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel> locationService, ILogger<LocationController> logger) : base(locationService, logger, "Location")
        {
        }
    }
}

