using Select2.Model;
using System.ComponentModel;

namespace ViewModels.Common.Company
{
    public class CompanyBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public CompanyBriefViewModel() : base(true, "The Company field is required.")
        {

        }
        [DisplayName("Company")]
        public override string? Name { get; set; }
    }

}
