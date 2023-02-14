using Models.Common.Interfaces;
using Select2.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Unit;
using ViewModels.MultiSelectInterfaces;
using ViewModels.Shared;

namespace ViewModels.Common.Department
{
    public class DepartmentDetailViewModel : BaseCrudViewModel, ISelect2Data, IUnitMultiSelect
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "At least one Unit required.")]
        public List<long> UnitIds { get; set; } = new List<long>();
        public List<UnitBriefViewModel> Units { get; set; } = new List<UnitBriefViewModel>();
        public string FormattedUnits
        {
            get
            {
                return Units != null && Units.Count() > 0 ? string.Join(", ", Units.Select(m => m.Name).ToList()) : "";
            }
        }
    }
}
