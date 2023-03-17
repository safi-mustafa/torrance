using Enums;
using Helpers.Extensions;
using Models.OverrideLogs;
using Models.TimeOnTools;

namespace ViewModels.Notification
{
    public class LogPushNotificationViewModel
    {
        private readonly NotificationViewModel _notification;
        public long LogId { get; set; }
        public long EntityId { get; set; }

        public string EntityType { get; set; }
        public LogType LogType { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public LogPushNotificationViewModel() { }
        public LogPushNotificationViewModel(NotificationViewModel notification)
        {
            _notification = notification;
            LogId = notification.LogId;
            EntityId = notification.LogId;
            EntityType = notification.EntityType.ToString();
            LogType = GetLogType(_notification.EntityType);
            Title =  GetTitle();
            Message = GetMessage();
        }
        public string GetTitle()
        {
            return $"{LogType.GetDisplayName()} {_notification.EventType.ToString().ToLower()}";
        }
        public string GetMessage()
        {
            return $"{_notification.EntityType.GetDisplayName()} with {_notification.IdentifierKey} ({_notification.IdentifierValue}) has been {_notification.EventType.ToString().ToLower()}";
        }

        private LogType GetLogType(NotificationEntityType type)
        {
            return type == NotificationEntityType.TOTLog ? LogType.TimeOnTools : type == NotificationEntityType.OverrideLog ? LogType.Override : LogType.WeldingRodRecord;
        }
    }
}

