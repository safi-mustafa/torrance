using Enums;
using Helpers.Models.Shared;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Newtonsoft.Json;

namespace ViewModels.Notification
{
    public class NotificationViewModel
    {
        public NotificationViewModel()
        {

        }
        public NotificationViewModel(long logId, Type logType, string sendTo, string message, NotificationType type, NotificationEntityType entityType)
        {
            var pushNotification = new PushNotificationViewModel
            {
                LogId = logId,
                LogType = GetLogType(logType),
                Message = message
            };

            Entity = logId;
            Message = JsonConvert.SerializeObject(pushNotification);
            SendTo = sendTo;
            Type = type;
            EntityType = entityType;
        }
        public long Entity { get; set; }
        public string Message { get; set; }
        public string? Subject { get; set; }
        public string SendTo { get; set; }
        public NotificationType Type { get; set; }
        public NotificationEntityType? EntityType { get; set; }


        private LogType GetLogType(Type entity)
        {
            return entity == typeof(TOTLog) ? LogType.TimeOnTools : entity is OverrideLog ? LogType.Override : LogType.WeldingRodRecord;
        }
    }
}
