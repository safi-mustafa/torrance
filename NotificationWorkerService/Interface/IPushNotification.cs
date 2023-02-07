using System;
namespace NotificationWorkerService.Interface
{
    public interface IPushNotification
    {
        Task<bool> SendPushNotification(string sendTo, string message);
    }
}

