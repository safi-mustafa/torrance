using System;
using ExpoCommunityNotificationServer.Client;
using ExpoCommunityNotificationServer.Models;
using NotificationWorkerService.Interface;

namespace NotificationWorkerService.Repository
{
    public class PushNotificationService : IPushNotification
    {
        public PushNotificationService()
        {
        }
        public async Task<bool> SendPushNotification(string sendTo, string message)
        {
            try
            {
                IPushApiClient _client = new PushApiClient(sendTo);
                PushTicketRequest pushTicketRequest = new PushTicketRequest()
                {
                    //PushTo = new List<string>() { ... },
                    PushTitle = "TEST 1",
                    PushBody = "TEST 1",
                    PushChannelId = "test"
                };

                PushTicketResponse result = await _client.SendPushAsync(pushTicketRequest);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}

