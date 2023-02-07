using System;
using DataLibrary;
using Expo.Server.Client;
using Expo.Server.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using NotificationWorkerService.Interface;
using ViewModels.Notification;

namespace NotificationWorkerService.Repository
{
    public class PushNotificationService : IPushNotification
    {
        private readonly ToranceContext _db;
        private readonly ILogger<PushNotificationService> _logger;

        public PushNotificationService(ToranceContext db, ILogger<PushNotificationService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> SendPushNotification(Notification notification)
        {
            try
            {
                var deviceId = await _db.Users.Where(x => x.Id == int.Parse(notification.SendTo) && x.IsDeleted == false).Select(x => x.DeviceId).FirstOrDefaultAsync();

                var notificationBody = JsonConvert.DeserializeObject<PushNotificationViewModel>(notification.Message);

                var expoSDKClient = new PushApiClient();
                var pushTicketReq = new PushTicketRequest()
                {
                    PushBadgeCount = 7,
                    PushData = notificationBody,
                    PushTitle = notificationBody.Title,
                    PushBody = notificationBody.Message
                };
                var result = await expoSDKClient.PushSendAsync(pushTicketReq);
                _logger.LogInformation($"Entity Id: {notification.Entity}");
                if (result?.PushTicketErrors?.Count() > 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}

