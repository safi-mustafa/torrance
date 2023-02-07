using Enums;
using Helpers.Models.Shared;

namespace Models
{
    public class Notification : BaseDBModel
    {
        public string Message { get; set; }
        public string? Subject { get; set; }
        public string SendTo { get; set; }
        public long ResendCount { get; set; }
        public bool IsSent { get; set; }
        public NotificationType Type { get; set; }
        public long? Entity { get; set; }
        public NotificationEntityType? EntityType { get; set; }
    }
}
