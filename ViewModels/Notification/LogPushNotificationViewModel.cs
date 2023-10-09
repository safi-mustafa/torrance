using Enums;
using Helpers.Extensions;
using Models;
using Models.OverrideLogs;
using Models.TimeOnTools;

namespace ViewModels.Notification
{
    public class LogPushNotificationViewModel
    {
        private readonly NotificationViewModel _notification;
        private readonly NotificationSendToModel _sendTo;

        public long LogId { get; set; }
        public long EntityId { get; set; }

        public string EntityType { get; set; }
        public LogType LogType { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public LogPushNotificationViewModel(NotificationViewModel notification, NotificationSendToModel sendTo)
        {
            _notification = notification;
            _sendTo = sendTo;
            LogId = notification.LogId;
            EntityId = notification.LogId;
            EntityType = notification.EntityType.ToString();
            LogType = GetLogType(_notification.EntityType);
            Title = GetTitle();
            Message = GetMessage();

        }
        public string GetTitle()
        {
            return $"{LogType.GetDisplayName()} {_notification.EventType.GetDisplayName().ToString().ToLower()}";
        }
        public string GetMessage()
        {
            if (_notification.EventType == NotificationEventTypeCatalog.Created)
            {
                return GetCreatedMessage();
            }
            else if (_notification.EventType == NotificationEventTypeCatalog.Updated)
            {
                return GetUpdateMessage();
            }
            else if (_notification.EventType == NotificationEventTypeCatalog.ApproverAssigned)
            {
                return GetApproverAssignedMessage();
            }
            else
            {
                return GetApproveOrRejectMessage();
            }
        }
        public string GetCreatedMessage()
        {
            return $"{_notification.EntityType.GetDisplayName()} with {_notification.IdentifierKey} ({_notification.IdentifierValue}) has been {_notification.EventType.ToString().ToLower()}.";
        }

        public string GetUpdateMessage()
        {
            return $"{_notification.EntityType.GetDisplayName()} with {_notification.IdentifierKey} ({_notification.IdentifierValue}) has been {_notification.EventType.ToString().ToLower()}.";
        }

        public string GetApproverAssignedMessage()
        {
            if (_sendTo.Id == _notification.ApproverId)// Sending Notification To Assigned Approver
            {
                return $"You have beed assgined to {_notification.EntityType.GetDisplayName()} with {_notification.IdentifierKey} ({_notification.IdentifierValue}).";
            }
            else // Sending Notification to User Who Created this Log
            {
                return $"Your {_notification.EntityType.GetDisplayName()} with {_notification.IdentifierKey} ({_notification.IdentifierValue}) has been assigned to approver.";
            }

        }
        public string GetApproveOrRejectMessage()
        {
            return $"Your {_notification.EntityType.GetDisplayName()} request for {_notification.IdentifierKey} ({_notification.IdentifierValue}) has been {_notification.EventType.ToString().ToLower()}.";
        }

        private LogType GetLogType(NotificationEntityType type)
        {
            return type == NotificationEntityType.TOTLog ? LogType.TimeOnTools : type == NotificationEntityType.OverrideLog ? LogType.Override : LogType.WeldingRodRecord;
        }
    }
}

