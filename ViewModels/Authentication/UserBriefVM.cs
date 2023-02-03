using Select2.Model;
using System.ComponentModel;
using ViewModels;

namespace ViewModels.Authentication
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
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //private string _name;
        //public new string Name { get { return string.IsNullOrEmpty(_name) ? $"{FirstName} {LastName}" : _name; } set { _name = value; } }
        //public string Role { get; set; }
    }
    public class ApproverBriefViewModel : BaseBriefVM
    {

        public ApproverBriefViewModel(bool isValidationEnabled = false) : base(isValidationEnabled, "The Approver field is required.")
        {
            IsValidationEnabled = isValidationEnabled;
        }
        [DisplayName("Approver")]
        public override string Name { get; set; }
        public bool IsValidationEnabled { get; set; } = true;
    }

}
