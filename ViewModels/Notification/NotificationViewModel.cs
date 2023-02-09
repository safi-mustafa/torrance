using Enums;
using Helpers.Models.Shared;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Newtonsoft.Json;
using Select2.Model;
using ViewModels.Shared;

namespace ViewModels.Notification
{
    public class NotificationModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        public NotificationModifyViewModel()
        {

        }
        public NotificationModifyViewModel(long logId, Type logType, string sendTo, string title, string message, NotificationType type)
        {
            var pushNotification = new PushNotificationViewModel
            {
                LogId = logId,
                EntityId = logId,
                LogType = GetLogType(logType),
                Message = message,
                Title = title
            };

            EntityId = logId;
            EntityType = GetNotificationEntityType(pushNotification.LogType);
            pushNotification.EntityType = EntityType.ToString();
            Message = JsonConvert.SerializeObject(pushNotification);
            SendTo = sendTo;
            Type = type;

        }
        public long EntityId { get; set; }
        public string Message { get; set; }
        public string? Subject { get; set; }
        public string SendTo { get; set; }
        public NotificationType Type { get; set; }
        public NotificationEntityType? EntityType { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FormattedCreatedOn { get => CreatedOn.ToString("U"); }

        private LogType GetLogType(Type entity)
        {
            return entity == typeof(TOTLog) ? LogType.TimeOnTools : entity == typeof(OverrideLog) ? LogType.Override : LogType.WeldingRodRecord;
        }
        private NotificationEntityType GetNotificationEntityType(LogType logType)
        {
            return logType == LogType.Override ? NotificationEntityType.OverrideLog : NotificationEntityType.TOTLog;
        }
    }
}
