using Enums;
using Helpers.Datetime;
using Helpers.Models.Shared;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Newtonsoft.Json;
using Select2.Model;
using ViewModels.Shared;

namespace ViewModels.Notification
{
    public class NotificationViewModel
    {
        public NotificationViewModel() { }

        public Guid Id { get; set; }
        public long LogId { get; set; }
        public string IdentifierKey { get; set; }
        public string IdentifierValue { get; set; }
        public string SendTo { get; set; }
        public string Title { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; }

        public long? RequestorId { get; set; }

        public string User { get; set; }
        public NotificationType Type { get; set; }
        public long EntityId { get; set; }
        public NotificationEventTypeCatalog EventType { get; set; }
        public NotificationEntityType EntityType { get; set; } // For Identifying DB Table. It has nothing to do with log 
        public DateTime CreatedOn { get; set; }
        public string FormattedCreatedOn { get => CreatedOn.FormatDatetimeInPST(); }




    }
}
