using System;
using Enums;

namespace ViewModels.Notification
{
    public class PushNotificationViewModel
    {
        public PushNotificationViewModel()
        {
        }
        public long LogId { get; set; }
        public LogType LogType { get; set; }
        public string Message { get; set; }
    }
}

