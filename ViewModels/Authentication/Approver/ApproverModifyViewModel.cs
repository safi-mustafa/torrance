using ViewModels.Authentication.User;

namespace ViewModels.Authentication.Approver
{
    public class ApproverModifyViewModel : UserUpdateViewModel, IApproverAssociationsViewModel
    {
        public List<ApproverAssociationsViewModel> Associations { get; set; } = new List<ApproverAssociationsViewModel>();

    }
}
