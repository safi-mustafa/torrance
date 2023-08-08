using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Validation;

namespace ViewModels
{
    public class FCOReasonModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IValidateName
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
