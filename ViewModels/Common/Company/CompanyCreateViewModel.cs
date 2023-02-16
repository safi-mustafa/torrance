using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Validation;

namespace ViewModels.Common.Company
{
    public class CompanyCreateViewModel : BaseCreateVM, IBaseCrudViewModel, IValidateName
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
