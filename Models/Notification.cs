using System.ComponentModel.DataAnnotations;
using Enums;
using Helpers.Models.Shared;
using Models.Common.Interfaces;

namespace Models
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public string Message { get; set; }
        public string? Subject { get; set; }
        public string SendTo { get; set; }
        public long ResendCount { get; set; }
        public bool IsSent { get; set; }
        public NotificationType Type { get; set; }
        public long? EntityId { get; set; }
        public NotificationEntityType? EntityType { get; set; }
    }
}
