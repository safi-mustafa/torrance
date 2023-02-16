using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Validation;

namespace ViewModels.OverrideLogs
{
    public class ReasonForRequestModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IValidateName
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Reason for Request")]
        public string Name { get; set; }
    }
}
