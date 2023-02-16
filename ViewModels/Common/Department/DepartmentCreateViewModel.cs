using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Unit;
using ViewModels.MultiSelectInterfaces;
using ViewModels.Common.Validation;

namespace ViewModels.Common.Department
{
    public class DepartmentCreateViewModel : BaseCreateVM, IBaseCrudViewModel, IUnitMultiSelect, IValidateName
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(200)]
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
