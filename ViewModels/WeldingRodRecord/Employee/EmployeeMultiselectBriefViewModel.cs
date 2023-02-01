using Helper.Attributes;

namespace ViewModels.WeldingRodRecord
{
    public class EmployeeMultiselectBriefViewModel
    {

        public string ErrorMessage { get; } = "At least one Employee is Required.";
        public bool IsValidationEnabled { get => true; }

        [RequiredMultiSelect2("ErrorMessage", "IsValidationEnabled")]
        public List<long> EmployeeIds { get; set; } = new();
        public List<EmployeeBriefViewModel> Employees { get; set; } = new();
    }

}
