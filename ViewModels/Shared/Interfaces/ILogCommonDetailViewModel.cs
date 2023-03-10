using ViewModels.Authentication.User;

namespace ViewModels.Shared
{
    public interface ILogCommonDetailViewModel
    {
        bool IsUnauthenticatedApproval { get; set; }
        Guid NotificationId { get; set; }
        ApproverBriefViewModel Approver { get; set; }
    }
}