using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
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

        [DisplayName("Access Code")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Access Code must be of 4-digits.")]
        [Remote(action: "ValidateAccessCode", controller: "Employee", AdditionalFields = "Id,AccessCode", ErrorMessage = "Access Code already in use.")]
        public string AccessCode { get; set; }

        [Required]
        [Display(Name = "Confirm Access Code")]
        [Compare("AccessCode", ErrorMessage = "Confirm Access Code Does not Match with Access Code")]
        public string ConfirmAccessCode { get; set; }
    }
}
