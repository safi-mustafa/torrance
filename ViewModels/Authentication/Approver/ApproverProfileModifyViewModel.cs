using System.ComponentModel.DataAnnotations;
using Models.Common.Interfaces;
using ViewModels.Common.Unit;
using ViewModels.MultiSelectInterfaces;
using ViewModels.Shared;

namespace ViewModels.Authentication.Approver
{
    public class ApproverProfileModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[0-9]{7,15}$", ErrorMessage = "Phone no. must be a valid number")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "At least one Unit required.")]
        public List<long> UnitIds { get => Units?.Select(x => x.Id ?? 0).ToList() ?? new List<long>(); }
        public List<UnitBriefViewModel> Units { get; set; } = new List<UnitBriefViewModel>();
    }
}
