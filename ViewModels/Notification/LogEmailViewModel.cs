using Enums;
using Helpers.Extensions;
using Models.Common;
using Models.OverrideLogs;
using Models.TimeOnTools;

namespace ViewModels.Notification
{
    public class LogEmailViewModel : EmailBaseModel
    {
        private readonly NotificationViewModel _notification;
        private readonly ApproverAssociation? _approver;
        private readonly string _approvalLink;

        public LogEmailViewModel(NotificationViewModel notification, ApproverAssociation? approver,string approvalLink)
        {
            _notification = notification;
            _approver = approver;
            _approvalLink = approvalLink;
            Subject = GetSubject();
            Body = GetEmailBody();
        }
        public string GetSubject()
        {
            return $"{_notification.EntityType.GetDisplayName()} request submitted";
        }
        public string GetEmailBody()
        {
            return $@"<div class=""container"">
						<p>Dear {_approver?.Approver?.FullName},</p>
						<p>A new <strong>{_notification.EntityType.GetDisplayName()}</strong> request is submitted by <strong>{_notification.User}</strong> from department (<strong>{_approver?.Department?.Name}</strong>) and unit (<strong>{_approver?.Unit?.Name}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
						<p>Please click <a href=""{_approvalLink}"">here</a> to approve/reject this request. </p>
						<p>Best regards,</p>
						<p>BainBridge - Time on Tools</p>
					</div>";
        }

        private LogType GetLogType(NotificationEntityType type)
        {
            return type == NotificationEntityType.TOTLog ? LogType.TimeOnTools : type == NotificationEntityType.OverrideLog ? LogType.Override : LogType.WeldingRodRecord;
        }
    }
}

