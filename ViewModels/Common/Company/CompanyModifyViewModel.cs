using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;
using ViewModels.MultiSelectInterfaces;
using ViewModels.Common.Validation;

namespace ViewModels.Common.Company
{
    public class CompanyModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, ICraftMultiSelect, IValidateName
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "At least one Craft is required.")]
        public List<long> CraftIds { get; set; } = new List<long>();
        public List<BaseBriefVM> Crafts { get; set; } = new List<BaseBriefVM>();
        public string FormattedCrafts
        {
            get
            {
                return Crafts != null && Crafts.Count() > 0 ? string.Join(", ", Crafts.Select(m => m.Name).ToList()) : "";
            }
        }
    }
}
