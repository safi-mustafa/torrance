using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class CraftRateModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [Required]
        [DisplayName("Rate")]
        [Range(1, float.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public float Rate { get; set; }
    }
}
