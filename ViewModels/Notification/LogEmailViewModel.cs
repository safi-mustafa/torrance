using Enums;
using Microsoft.Extensions.Configuration;

namespace ViewModels.Notification
{
    public class LogEmailViewModel : EmailBaseModel
    {
        private readonly NotificationViewModel _notification;
        private readonly NotificationSendToModel _sendTo;
        private readonly string _publicLink;
        private readonly IConfiguration _configuration;
        private readonly Guid _notificationId;

        public LogEmailViewModel(NotificationViewModel notification, NotificationSendToModel sendTo, Guid notificationId, IConfiguration configuration)
        {
            _notification = notification;
            _sendTo = sendTo;
            _configuration = configuration;
            _notificationId = notificationId;
            _publicLink = GetPublicLink();
            Subject = GetSubject();
            Body = GetEmailBody();
        }


        public string GetSubject()
        {
            if (_notification.EventType == NotificationEventTypeCatalog.Created)
            {
                return $"{_notification.EntityType.GetDisplayName()} request submitted";
            }
            else if (_notification.EventType == NotificationEventTypeCatalog.Updated)
            {
                return $"{_notification.EntityType.GetDisplayName()} request updated by {_notification.User}";
            }
            else if (_notification.EventType == NotificationEventTypeCatalog.Approved || _notification.EventType == NotificationEventTypeCatalog.Rejected)
            {
                return $"{_notification.EntityType.GetDisplayName()} log with {_notification.IdentifierKey}-{_notification.IdentifierValue} {_notification.EventType}";
            }
            return "";

        }
        public string GetEmailBody()
        {
            if (_notification.EventType == NotificationEventTypeCatalog.Created)
            {
                return $@"<div class=""container"">
						<p>Dear {_sendTo.Name},</p>
						<p>A new <strong>{_notification.EntityType.GetDisplayName()}</strong> request is submitted by <strong>{_notification.Requestor}</strong> from department (<strong>{_notification.Department}</strong>) and unit (<strong>{_notification.Unit}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
					</div>";
                //<p>Please click <a href=""{_publicLink}"">here</a> to approve/reject this request. </p>
            }
            else if (_notification.EventType == NotificationEventTypeCatalog.Updated)
            {
                return $@"<div class=""container"">
						<p>Dear {_sendTo.Name},</p>
						<p>A <strong>{_notification.EntityType.GetDisplayName()}</strong> request is updated by <strong>{_notification.User}</strong> from department (<strong>{_notification.Department}</strong>) and unit (<strong>{_notification.Unit}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
					</div>";
            }
            if (_notification.EventType == NotificationEventTypeCatalog.ApproverAssigned)
            {
                if (_sendTo.Id == _notification.ApproverId) //send to assigned approver
                {
                    return $@"<div class=""container"">
						<p>Dear {_sendTo.Name},</p>
						<p>You have been assigned to <strong>{_notification.EntityType.GetDisplayName()}</strong> submitted by <strong>{_notification.Requestor}</strong> from department (<strong>{_notification.Department}</strong>) and unit (<strong>{_notification.Unit}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
                        <p>Please click <a href=""{_publicLink}"">here</a> to approve/reject this request. </p>
					</div>";
                }
                else// Send to requestor
                {
                    return $@"<div class=""container"">
						<p>Dear {_sendTo.Name},</p>
						<p>Your request for <strong>{_notification.EntityType.GetDisplayName()}</strong> is assigned to approver for department (<strong>{_notification.Department}</strong>) and unit (<strong>{_notification.Unit}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
					</div>";
                    //<p>Please click <a href=""{_publicLink}"">here</a> to approve/reject this request. </p>
                }

            }
            else if (_notification.EventType == NotificationEventTypeCatalog.Approved || _notification.EventType == NotificationEventTypeCatalog.Rejected)
            {
                if (_notification.RequestorId.ToString() == _sendTo.Id)
                {
                    return $@"<div class=""container"">
						<p>Dear {_sendTo.Name},</p>
						<p>Your <strong>{_notification.EntityType.GetDisplayName()}</strong> request is <strong>{_notification.EventType}</strong> by <strong>{_notification.User}</strong> from department (<strong>{_notification.Department}</strong>) and unit (<strong>{_notification.Unit}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
					</div>";
                }
                else
                {
                    return $@"<div class=""container"">
						<p>Dear {_sendTo.Name},</p>
						<p><strong>{_notification.EntityType.GetDisplayName()}</strong> request is <strong>{_notification.EventType}</strong> by <strong>{_notification.User}</strong> from department (<strong>{_notification.Department}</strong>) and unit (<strong>{_notification.Unit}</strong>) under {_notification.IdentifierKey} (<strong>{_notification.IdentifierValue}</strong>).</p>
					</div>";
                }

                //         <p>Best regards,</p>
                //<p>Torrance</p>
            }
            return "";

            //         <p>Best regards,</p>
            //<p>Torrance - Time on Tools</p>
        }

        private LogType GetLogType(NotificationEntityType type)
        {
            return type == NotificationEntityType.TOTLog ? LogType.TimeOnTools : type == NotificationEntityType.OverrideLog ? LogType.Override : LogType.WeldingRodRecord;
        }

        public string GetPublicLink()
        {
            if (_notification.EventType == NotificationEventTypeCatalog.ApproverAssigned)
            {
                var domain = _configuration["WebUrl"];
                return $"{domain}/Approval/ApproveByNotification?id={_notificationId}";
            }
            return null;
        }
    }
}

