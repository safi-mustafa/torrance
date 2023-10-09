using System;
using System.ComponentModel.DataAnnotations;
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

        public bool IsEditRestricted
        {
            get
            {
                return Status != Status.Pending;
            }
        }

        [Display(Name = "Is Archived")]
        public bool IsArchived { get; set; }
        public new string FormattedStatus
        {
            get
            {
                return IsArchived ? "IsArchived" : Status.GetDisplayName();
            }
        }
        public string FormattedStatusForView { get { return IsArchived ? "IsArchived" : Status.ToString(); } }

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel();
    }
}

