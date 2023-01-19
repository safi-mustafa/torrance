using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.Location;
using Repositories.Services.WeldRodRecordServices.LocationService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : CrudBaseBriefController<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel, LocationDetailViewModel, LocationSearchViewModel>
    {
        public LocationController(ILocationService<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel> locationService) : base(locationService)
        {
        }
    }
}

