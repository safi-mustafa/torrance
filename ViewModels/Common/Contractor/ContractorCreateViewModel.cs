using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.Common.Contractor
{
    public class ContractorCreateViewModel : BaseCreateVM, IBaseCrudViewModel
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Composite Rate")]
        public double CompositeRate { get; set; }
    }
}
