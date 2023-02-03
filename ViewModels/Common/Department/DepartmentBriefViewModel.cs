using Select2.Model;
using System.ComponentModel;

namespace ViewModels.Common.Department
{
    public class DepartmentBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public DepartmentBriefViewModel() : base(false, "The Department field is required.")
        {

        }
        [DisplayName("Department")]
        public override string? Name { get; set; }
    }

}
