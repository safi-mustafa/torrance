using System;
namespace NotificationWorkerService.Repository
{
    public class PushNotificationService
    {
        public PushNotificationService()
        {
        }
        public async Task SendPushNotification()
        {
            IPushApiClient _client = new PushApiClient("your token here");
            PushTicketRequest pushTicketRequest = new PushTicketRequest()
            {
                PushTo = new List<string>() { ... },
                PushTitle = "TEST 1",
                PushBody = "TEST 1",
                PushChannelId = "test"
            };

            PushTicketResponse result = await _client.SendPushAsync(pushTicketRequest);
        }
    }

}

