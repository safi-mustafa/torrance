using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices.MobileFileServices;
using ViewModels.AppSettings.MobileFiles.BadgeRoom;

namespace Web.Controllers
{
    [Authorize]
    
    public class BadgeRoomController : CrudBaseController<BadgeRoomModifyViewModel, BadgeRoomModifyViewModel, BadgeRoomDetailViewModel, BadgeRoomDetailViewModel, BadgeRoomSearchViewModel>
    {
        private readonly IMobileFileService<BadgeRoomModifyViewModel, BadgeRoomModifyViewModel, BadgeRoomDetailViewModel> _badgeRoomService;
        private readonly ILogger<BadgeRoomController> _logger;

        public BadgeRoomController(IMobileFileService<BadgeRoomModifyViewModel, BadgeRoomModifyViewModel, BadgeRoomDetailViewModel> badgeRoomService, ILogger<BadgeRoomController> logger, IMapper mapper) : base(badgeRoomService, logger, mapper, "BadgeRoom", "BadgeRooms")
        {
            _badgeRoomService = badgeRoomService;
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
