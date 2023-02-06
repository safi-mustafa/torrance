using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Unit;
using ViewModels.MultiSelectInterfaces;

namespace ViewModels.Authentication.Approver
{
    public class ApproverModifyViewModel : UserUpdateViewModel, IUnitMultiSelect
    {
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
