using Select2.Model;
using System.ComponentModel;
using ViewModels.Common.Company;

namespace ViewModels.WeldingRodRecord
{
    public class EmployeeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public EmployeeBriefViewModel() : base(true, "The Requester field is required.")
        {

        }

        public EmployeeBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Requester field is required.")
        {

        }
        public EmployeeBriefViewModel(bool isValidationEnabled, string message) : base(isValidationEnabled, message)
        {

        }
        [DisplayName("Employee")]
        public override string? Name { get; set; }
        public string? Email { get; set; }

    }

}
