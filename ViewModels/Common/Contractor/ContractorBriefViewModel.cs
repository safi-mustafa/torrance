using Select2.Model;
using System.ComponentModel;

namespace ViewModels.Common.Contractor
{
    public class ContractorBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ContractorBriefViewModel() : base(false, "")
        {

        }
        [DisplayName("Contractor")]
        public override string? Name { get; set; }
    }

}
