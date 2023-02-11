using Select2.Model;
using System.ComponentModel;
using ViewModels.Common.Company;

namespace ViewModels.AppSettings.CompanyManager
{
    public class CompanyManagerBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public CompanyManagerBriefViewModel() : base(true, "The Company Manager field is required.")
        {

        }

        public CompanyManagerBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Company Manager field is required.")
        {

        }
        [DisplayName("Company Manager")]
        public override string? Name { get; set; }

    }

}
