using Enums;

namespace ViewModels.Notification
{
    public class NotificationViewModel
    {
        public long EntityId { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string SendTo { get; set; }
        public NotificationType Type { get; set; }
    }
}
