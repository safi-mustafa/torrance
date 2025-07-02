using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Unit;
using ViewModels.MultiSelectInterfaces;

namespace ViewModels.Authentication.Approver
{
    public class ApproverModifyViewModel : UserUpdateViewModel, IApproverAssociationsViewModel, IUnitMultiSelect
    {
        public List<ApproverAssociationsViewModel> Associations { get; set; } = new List<ApproverAssociationsViewModel>();

        public new CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(false, "");

        // Properties for IUnitMultiSelect implementation
        public List<long> UnitIds { get; set; } = new List<long>();
        public List<UnitBriefViewModel> Units { get; set; } = new List<UnitBriefViewModel>();
    }
}
