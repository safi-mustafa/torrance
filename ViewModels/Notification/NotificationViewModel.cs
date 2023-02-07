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
        public NotificationViewModel(Status status, long logId, Type logType, string sendTo, string message)
        {
            var pushNotification = new PushNotificationViewModel
            {
                LogId = logId,
                LogType = GetLogType(logType),
                Message = message
            };

            EntityId = logId;
            Message = JsonConvert.SerializeObject(pushNotification);
            SendTo = sendTo;
        }
        public long EntityId { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string SendTo { get; set; }
        public NotificationType Type { get; set; }


        private LogType GetLogType(Type entity)
        {
            return entity is TOTLog ? LogType.TimeOnTools : entity is OverrideLog ? LogType.Override : LogType.WeldingRodRecord;
        }
    }
}
