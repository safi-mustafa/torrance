using Enums;
using Pagination;

namespace ViewModels.Notification
{
    public class NotificationSearchViewModel : BaseSearchModel
    {
        public long Id { get; set; }
        public NotificationType? Type { get; set; }
        public bool? IsSent { get; set; }
    }
}
