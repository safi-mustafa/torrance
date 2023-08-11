using Select2.Model;
using System.ComponentModel;
using ViewModels;

namespace ViewModels.Authentication.User
{
    public class UserBriefViewModel : BaseBriefVM
    {
        public UserBriefViewModel() : base(false, "")
        {
        }
        public UserBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled)
        {
        }
        public UserBriefViewModel(bool isValidationEnabled, string errorMessage) :
            base(isValidationEnabled, errorMessage)
        {
        }
        public override string? Name { get; set; }
    }
    public class ApproverBriefViewModel : BaseBriefVM, IApproverBaseBriefViewModel
    {
        public ApproverBriefViewModel() : base(false, "")
        {
        }
        public ApproverBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Approver field is required.")
        {
        }

        [DisplayName("Approver")]
        public override string? Name { get; set; }
    }
    public class DesignatedCoordinatorBriefViewModel : BaseBriefVM, IApproverBaseBriefViewModel
    {
        public DesignatedCoordinatorBriefViewModel() : base(false, "")
        {
        }
        public DesignatedCoordinatorBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Authorize For Immediate Start field is required.")
        {
        }

        [DisplayName("Designated Coordinator")]
        public override string? Name { get; set; }
    }
    public class RLTMemberBriefViewModel : BaseBriefVM, IApproverBaseBriefViewModel
    {
        public RLTMemberBriefViewModel() : base(false, "")
        {
        }
        public RLTMemberBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The RLT Member field is required.")
        {
        }

        [DisplayName("Endorse: RLT Member")]
        public string? Name { get; set; }
    }
    public class BTLBriefViewModel : BaseBriefVM, IApproverBaseBriefViewModel
    {
        public BTLBriefViewModel() : base(false, "")
        {
        }
        public BTLBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The BTL field is required.")
        {
        }

        [DisplayName("Endorse: BTL")]
        public string? Name { get; set; }
    }
    public class TELBriefViewModel : BaseBriefVM, IApproverBaseBriefViewModel
    {
        public TELBriefViewModel() : base(false, "")
        {
        }
        public TELBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The TEL field is required.")
        {
        }

        [DisplayName("Approver: TEL (Turnaround Event Lead)")]
        public string? Name { get; set; }
    }
    public class MaintManagerBriefViewModel : BaseBriefVM, IApproverBaseBriefViewModel
    {
        public MaintManagerBriefViewModel() : base(false, "")
        {
        }
        public MaintManagerBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Maint Manager field is required.")
        {
        }

        [DisplayName("Approver: (Maint Manager)")]
        public string? Name { get; set; }
    }

}
