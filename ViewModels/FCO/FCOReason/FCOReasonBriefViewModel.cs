using Select2.Model;
using System.ComponentModel;

namespace ViewModels
{
    public class FCOReasonBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public FCOReasonBriefViewModel() : base(true, "The FCO Reason field is required.")
        {

        }
        [DisplayName("FCO Reason")]
        public override string Name { get; set; }
    }

}
