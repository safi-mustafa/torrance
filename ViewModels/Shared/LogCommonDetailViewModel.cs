using System;
using System.ComponentModel.DataAnnotations;
using Enums;
using Helpers.Extensions;
using ViewModels.Authentication.User;
using ViewModels.Shared.Interfaces;
using ViewModels.WeldingRodRecord;

namespace ViewModels.Shared
{
    public class LogCommonDetailViewModel : BaseCrudViewModel, ILogCommonDetailViewModel, ILoggedInUserRole
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

        public string LoggedInUserRole { get; set; }
        public long LoggedInUserId { get; set; }
        public bool CanProcess
        {
            get
            {
                if (LoggedInUserRole == RolesCatalog.SuperAdmin.ToString() || LoggedInUserRole == RolesCatalog.Administrator.ToString() || LoggedInUserRole == RolesCatalog.Approver.ToString())
                {
                    return Status == Status.Pending || Status == Status.InProcess;
                }

                return false;
            }
        }

        public bool CanUpdate
        {
            get
            {
                if (LoggedInUserRole == RolesCatalog.SuperAdmin.ToString() || LoggedInUserRole == RolesCatalog.Administrator.ToString())
                {
                    return true;
                }
                if (
                        (LoggedInUserRole == RolesCatalog.Employee.ToString() || LoggedInUserRole == RolesCatalog.Approver.ToString())
                        && (Status == Status.Pending || Status == Status.InProcess)
                        && Employee?.Id == LoggedInUserId
                   )
                {
                    return true;
                }
                return false;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (LoggedInUserRole == RolesCatalog.SuperAdmin.ToString() || LoggedInUserRole == RolesCatalog.Administrator.ToString())
                {
                    return true;
                }
                if (
                        (LoggedInUserRole == RolesCatalog.Employee.ToString() || LoggedInUserRole == RolesCatalog.Approver.ToString())
                        && (Status == Status.Pending || Status == Status.InProcess)
                        && Employee?.Id == LoggedInUserId
                   )
                {
                    return true;
                }
                return false;
            }
        }

        public string FormattedStatusForView { get { return IsArchived ? "IsArchived" : Status.ToString(); } }

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false, "");

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel(false, "");
    }
}

