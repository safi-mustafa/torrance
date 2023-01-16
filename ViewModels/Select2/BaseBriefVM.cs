using Helper.Attributes;
using System.ComponentModel;

namespace ViewModels
{

    public interface IBaseBriefVM
    {
        long Id { get; set; }
        string Name { get; set; }
    }
    public class BaseBriefVM : IBaseBriefVM
    {
        public string ErrorMessage { get; } = "";
        public bool IsValidationEnabled { get; }

        public BaseBriefVM()
        {
            IsValidationEnabled = false;
            ErrorMessage = "";
        }
        public BaseBriefVM(bool isValidationEnabled)
        {
            IsValidationEnabled = isValidationEnabled;
            ErrorMessage = "";
        }
        public BaseBriefVM(bool isValidationEnabled, string errorMessage)
        {
            IsValidationEnabled = isValidationEnabled;
            ErrorMessage = errorMessage;
        }
        [RequiredSelect2("ErrorMessage", "IsValidationEnabled")]
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class BaseImageBriefVM : BaseBriefVM
    {
        public string ImageUrl { get; set; }
    }
    public class SupervisorBriefVM : BaseBriefVM
    {
        public SupervisorBriefVM() : base(true, "The Supervisor field is required.")
        {
        }
        [DisplayName("Supervisor")]
        public override string Name { get; set; }
    }

    public class CustomerBriefVM : BaseBriefVM
    {
        public CustomerBriefVM() : base(true, "The Customer field is required.")
        {

        }
        [DisplayName("Customer")]
        public override string Name { get; set; }
    }

    public class CraftBriefVM : BaseBriefVM
    {
        public CraftBriefVM() : base(true, "The Craft field is required.")
        {

        }
        [DisplayName("Craft")]
        public override string Name { get; set; }
    }

    public class SESApproverBriefVM : BaseBriefVM
    {
        public SESApproverBriefVM() : base(true, "The SESApprover field is required.")
        {

        }
        [DisplayName("SES Approver")]
        public override string Name { get; set; }
    }

    public class ContractBriefVM : BaseBriefVM
    {
        public ContractBriefVM() : base(true, "The Contract field is required.")
        {

        }
        [DisplayName("Contract")]
        public override string Name { get; set; }
    }

    public class EmployeeBriefVM : BaseBriefVM
    {
        public EmployeeBriefVM() : base(true, "The Employee field is required.")
        {

        }
        [DisplayName("Employee")]
        public override string Name { get; set; }
    }

    public class InputBriefVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

}
