using System;
using Enums;
using Helpers.Models.Shared;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Newtonsoft.Json;

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

