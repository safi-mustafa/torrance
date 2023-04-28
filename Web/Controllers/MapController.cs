using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.AppSettingServices.MappService;
using ViewModels.AppSettings.Map;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class MapController : CrudBaseController<MapModifyViewModel, MapModifyViewModel, MapDetailViewModel, MapDetailViewModel, MapSearchViewModel>
    {
        private readonly IMapService<MapModifyViewModel, MapModifyViewModel, MapDetailViewModel> _mapService;
        private readonly ILogger<MapController> _logger;

        public MapController(IMapService<MapModifyViewModel, MapModifyViewModel, MapDetailViewModel> mapService, ILogger<MapController> logger, IMapper mapper) : base(mapService, logger, mapper, "Map", "Maps")
        {
            _mapService = mapService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Latitude",data = "Latitude", orderable = true},
                new DataTableViewModel{title = "Longitude",data = "Longitude", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
