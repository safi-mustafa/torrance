using Enums;
using Helpers.Extensions;
using Models;
using Models.Common;
using Models.OverrideLogs;
using Models.TimeOnTools;
using ViewModels.Authentication.Approver;

namespace ViewModels.Notification
{
    public class LogEmailViewModel : EmailBaseModel
    {
        private readonly NotificationViewModel _notification;
        private readonly ToranceUser _sentUser;
        private readonly ApproverAssociationNotificationViewModel? _approver;
        private readonly string _approvalLink;
        public LogEmailViewModel()
        {

        }
        public LogEmailViewModel(NotificationViewModel notification, ApproverAssociationNotificationViewModel? approver, string approvalLink)
        {
            _notification = notification;
            _approver = approver;
            _approvalLink = approvalLink;
            Subject = GetSubject();
            Body = GetEmailBody();
        }
        //To send Requestor email after log processing. SentEmailType is there for future email types
        public LogEmailViewModel(NotificationViewModel notification, ApproverAssociationNotificationViewModel? approver, SentEmailType emailType)
        {
            _notification = notification;
            _approver = approver;
            Subject = GetProcessedLogSubject();
            Body = GetEmailBodyForProcessedLog();
        }


        public string GetEmailBodyForProcessedLog()
        {
            return $@"<div class=""container"">
						<p>Dear {_notification.User},</p>
						<p>Your <strong>{_notification.EntityType.GetDisplayName()}</strong> request is <strong>{_notification.EventType}</strong> by <strong>{_approver?.Approver?.Name}</strong> from department (<strong>{_approver?.Department?.Name}</strong>) and unit (<strong>{_approver?.Unit?.Name}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
					</div>";
            //         <p>Best regards,</p>
            //<p>Torrance</p>
        }
        public string GetProcessedLogSubject()
        {
            return $"{_notification.EntityType.GetDisplayName()} log with {_notification.IdentifierKey}-{_notification.IdentifierValue} {_notification.EventType}";
        }
        public string GetSubject()
        {
            return $"{_notification.EntityType.GetDisplayName()} request submitted";
        }
        public string GetEmailBody()
        {
            return $@"<div class=""container"">
						<p>Dear {_approver?.Approver?.Name},</p>
						<p>A new <strong>{_notification.EntityType.GetDisplayName()}</strong> request is submitted by <strong>{_notification.User}</strong> from department (<strong>{_approver?.Department?.Name}</strong>) and unit (<strong>{_approver?.Unit?.Name}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
						<p>Please click <a href=""{_approvalLink}"">here</a> to approve/reject this request. </p>
					</div>";
            //         <p>Best regards,</p>
            //<p>Torrance - Time on Tools</p>
        }

        private LogType GetLogType(NotificationEntityType type)
        {
            return type == NotificationEntityType.TOTLog ? LogType.TimeOnTools : type == NotificationEntityType.OverrideLog ? LogType.Override : LogType.WeldingRodRecord;
        }
    }
}

