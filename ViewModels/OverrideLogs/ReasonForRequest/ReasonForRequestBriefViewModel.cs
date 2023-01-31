using Select2.Model;
using System.ComponentModel;

namespace ViewModels.OverrideLogs
{
    public class ReasonForRequestBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ReasonForRequestBriefViewModel() : base(true, "The Reason for Request field is required.")
        {

        }
        [DisplayName("Reason for Request")]
        public override string Name { get; set; }
    }

}
