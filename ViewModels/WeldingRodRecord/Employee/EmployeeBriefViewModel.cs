using Select2.Model;
using System.ComponentModel;
using ViewModels.Common.Company;

namespace ViewModels.WeldingRodRecord
{
    public class EmployeeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public EmployeeBriefViewModel() : base(false, "")
        {

        }

        public EmployeeBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Employee field is required.")
        {

        }
        [DisplayName("Employee")]
        public override string? Name { get; set; }
        public string? Email { get; set; }

    }

}
