using Helpers.Extensions;
using ViewModels.Authentication.User;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs.ORLog;

namespace ViewModels.Authentication.Approver
{
    public class ApproverDetailViewModel : UserDetailViewModel, IApproverAssociationsViewModel
    {
        public List<ApproverAssociationsViewModel> Associations { get; set; } = new List<ApproverAssociationsViewModel>();
    }
}
