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
        public UserBriefViewModel(bool isValidationEnabled, string errorMessage) : base(isValidationEnabled, errorMessage)
        {
        }
        public override string? Name { get; set; }
    }
    public class ApproverBriefViewModel : BaseBriefVM
    {
        private string? name;

        public ApproverBriefViewModel() : base(false, "")
        {
        }
        public ApproverBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Approver field is required.")
        {
        }

        [DisplayName("Approver")]
        public override string? Name { get => name; set => name = value; }
    }

}
