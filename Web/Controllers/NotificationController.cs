using DataLibrary;
using Expo.Server.Client;
using Expo.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ToranceContext _db;

        public NotificationController(ToranceContext db)
        {
            _db = db;
        }
        public async Task<bool> SendBlankNotification()
        {
            try
            {

                var users = await _db.Users.Where(x => !string.IsNullOrEmpty(x.DeviceId)).ToListAsync();

                var notificationBody = "";

                var expoSDKClient = new PushApiClient();
                var pushTicketReq = new PushTicketRequest()
                {
                    PushBadgeCount = 0,
                    PushData = notificationBody,
                    PushTitle = notificationBody.Title,
                    PushBody = notificationBody.Message
                };

                pushTicketReq.PushTo = new List<string>() { deviceId };

                var result = await expoSDKClient.PushSendAsync(pushTicketReq);
                if (result == null || result?.PushTicketErrors?.Count() > 0)
                    return false;
                foreach (var stat in result?.PushTicketStatuses)
                {
                    if (stat.TicketStatus == "error")
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
