using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Validation;

namespace ViewModels.WeldingRodRecord.Location
{
    public class LocationModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IValidateName
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
