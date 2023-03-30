using ViewModels.Authentication.User;
using ViewModels.Common.Company;

namespace ViewModels.Authentication.Approver
{
    public class ApproverModifyViewModel : UserUpdateViewModel, IApproverAssociationsViewModel
    {
        public List<ApproverAssociationsViewModel> Associations { get; set; } = new List<ApproverAssociationsViewModel>();

        public new CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(false,"");

    }
}
