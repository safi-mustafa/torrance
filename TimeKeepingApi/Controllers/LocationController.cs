using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.Location;
using Repositories.Services.WeldRodRecordServices.LocationService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : CrudBaseBriefController<LocationDetailViewModel, LocationSearchViewModel>
    {
        public LocationController(ILocationService<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel> locationService) : base(locationService)
        {
        }
    }
}

