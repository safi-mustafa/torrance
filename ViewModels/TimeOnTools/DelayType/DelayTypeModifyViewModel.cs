using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.TimeOnTools
{
    public class DelayTypeModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
