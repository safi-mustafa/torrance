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
    public class NotificationViewModel : INotificationMetaViewModel
    {
        public NotificationViewModel() { }

        public Guid Id { get; set; }

        public long LogId { get; set; }
        public string SendTo { get; set; }
        public string Title { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; }

        public string User { get; set; }

        public NotificationType Type { get; set; }
        public long EntityId { get; set; }
        public NotificationEntityType EntityType { get; set; }
        public NotificationEventTypeCatalog EventType { get; set; }

        public string IdentifierKey { get; set; }
        public string IdentifierValue { get; set; }
        public string DepartmentId { get; set; }
        public string Department { get; set; }
        public string UnitId { get; set; }
        public string Unit { get; set; }

        public long? RequestorId { get; set; }

        public string Requestor { get; set; }
        public string ApproverId { get; set; }

        public string Approver { get; set; }

        public DateTime CreatedOn { get; set; }
        public string FormattedCreatedOn { get => CreatedOn.FormatDatetimeInPST(); }

    }

    public interface INotificationMetaViewModel
    {
        long LogId { get; set; }

        string DepartmentId { get; set; }
        string Department { get; set; }
        string UnitId { get; set; }
        string Unit { get; set; }
        long? RequestorId { get; set; }

        string Requestor { get; set; }
        string IdentifierKey { get; set; }
        string IdentifierValue { get; set; }

        NotificationEntityType EntityType { get; set; }

        string ApproverId { get; set; }

        string Approver { get; set; }


    }

    public class NotificationSendToModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool SendPushNotification { get; set; } = true;
    }
}
