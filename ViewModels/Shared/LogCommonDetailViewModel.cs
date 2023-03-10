using System;
using Enums;
using Helpers.Extensions;
using ViewModels.Authentication.User;

namespace ViewModels.Shared
{
    public class LogCommonDetailViewModel : BaseCrudViewModel, ILogCommonDetailViewModel
    {
        public bool IsUnauthenticatedApproval { get; set; }
        public Guid NotificationId { get; set; }
        public Status Status { get; set; }

        public string FormattedStatus { get => Status.GetDisplayName(); }
        public bool IsEditRestricted
        {
            get
            {
                return Status != Status.Pending;
            }
        }
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel();
    }
}

