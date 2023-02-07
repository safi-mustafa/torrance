using System;
using DataLibrary;
using Expo.Server.Client;
using Expo.Server.Models;
using Models;
using Newtonsoft.Json;
using NotificationWorkerService.Interface;

namespace NotificationWorkerService.Repository
{
    public class PushNotificationService : IPushNotification
    {
        private readonly ToranceContext _db;

        public PushNotificationService(ToranceContext db)
        {
            _db = db;
        }
        public async Task<bool> SendPushNotification(string sendTo, string message)
        {
            try
            {
                var deviceIds = await _db.Users.Where(x => x.Id == .Contains(x.UserId) && x.IsDeleted == false).Select(x => x.DeviceId).ToListAsync();

                var notificationBody = JsonConvert.DeserializeObject<PushNotificationModel>(message);
                notificationBody.TaskId = notificationBody.TaskId == 0 ? notification.EntityId : notificationBody.TaskId;
                var notificationFor = notificationBody.TaskType == AuditTaskType.WithCheckpoints ? "Audit" : "Task";

                var expoSDKClient = new PushApiClient();
                var pushTicketReq = new PushTicketRequest()
                {
                    PushBadgeCount = 7,
                    PushData = notificationBody,
                    PushTitle = notificationBody.Title,
                    PushBody = notificationBody.Message
                };
                var notificationStatus = PushNotificationStatus.Sent;
                pushTicketReq.PushTo = new List<string>() { sendTo };
                var result = await expoSDKClient.PushSendAsync(pushTicketReq);
                if (result?.PushTicketErrors?.Count() > 0)
                    notificationStatus = PushNotificationStatus.Pending;
                else
                    notificationStatus = PushNotificationStatus.Sent;
                notification.Status = PushNotificationStatus.Sent;
                _logger.LogInformation($"Entity Id: {notification.EntityId}");
                await _db.SaveChangesAsync();
                return true;
            }
            }
            catch (Exception ex)
            {
                return false;
            }
}

}

