using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.Common.Department
{
    public class DepartmentDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
