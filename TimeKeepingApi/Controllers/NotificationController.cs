﻿using Torrance.Api.Controllers;
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
    public class NotificationController : CrudBaseBriefController<NotificationViewModel, NotificationSearchViewModel>
    {
        public NotificationController(INotificationService permitTypeService, ILogger<NotificationController> logger) : base(permitTypeService, logger, "Notification")
        {
        }

        public override Task<IActionResult> GetAll([FromQuery] NotificationSearchViewModel search)
        {
            search.OrderByColumn = "CreatedOn";
            search.OrderDir = Pagination.PaginationOrderCatalog.Desc;
            return base.GetAll(search);
        }
    }
}

