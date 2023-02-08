using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ViewModels.TimeOnTools;
using ViewModels.Notification;
using Repositories.Shared.NotificationServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : CrudBaseBriefController<NotificationModifyViewModel, NotificationSearchViewModel>
    {
        public NotificationController(INotificationService<NotificationModifyViewModel, NotificationModifyViewModel, NotificationModifyViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

