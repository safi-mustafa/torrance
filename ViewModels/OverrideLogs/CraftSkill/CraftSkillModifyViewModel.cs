using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Validation;

namespace ViewModels.OverrideLogs
{
    public class CraftSkillModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IValidateName
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Skill")]
        public string Name { get; set; }

        [Required]
        [DisplayName("ST Rate")]
        [Range(1, double.MaxValue, ErrorMessage = "The ST Rate must be greater than zero.")]
        public double STRate { get; set; }

        [Required]
        [DisplayName("OT Rate")]
        [Range(1, double.MaxValue, ErrorMessage = "The OT Rate must be greater than zero.")]
        public double OTRate { get; set; }

        [Required]
        [DisplayName("DT Rate")]
        [Range(1, double.MaxValue, ErrorMessage = "The DT Rate must be greater than zero.")]
        public double DTRate { get; set; }
    }
}
