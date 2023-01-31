using Select2.Model;
using System.ComponentModel;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public EmployeeBriefViewModel() : base(true, "The Employee field is required.")
        {

        }
        [DisplayName("Employee")]
        public override string? Name { get; set; }
    }

}
