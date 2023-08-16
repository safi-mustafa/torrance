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
    public class DesignatedCoordinatorBriefViewModel : BaseBriefVM
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
    public class AreaExecutionLeadBriefViewModel : BaseBriefVM
    {
        public AreaExecutionLeadBriefViewModel() : base(false, "")
        {
        }
        public AreaExecutionLeadBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Area Execution Lead field is required.")
        {
        }

        [DisplayName("Area Execution Lead")]
        public string? Name { get; set; }
    }
    public class BusinessTeamLeaderBriefViewModel : BaseBriefVM
    {
        public BusinessTeamLeaderBriefViewModel() : base(false, "")
        {
        }
        public BusinessTeamLeaderBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Business Team Leader field is required.")
        {
        }

        [DisplayName("Business Team Leader")]
        public string? Name { get; set; }
    }
    public class RejecterBriefViewModel : BaseBriefVM
    {
        public RejecterBriefViewModel() : base(false, "")
        {
        }
        public RejecterBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Rejecter field is required.")
        {
        }

        [DisplayName("Rejecter")]
        public string? Name { get; set; }
    }

}
