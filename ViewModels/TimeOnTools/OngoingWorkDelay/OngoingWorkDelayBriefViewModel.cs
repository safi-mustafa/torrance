using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.OngoingWorkDelay
{
    public class OngoingWorkDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public OngoingWorkDelayBriefViewModel() : base(false, "")
        {

        }
        [DisplayName("Ongoing Work Delay")]
        public override string? Name { get; set; }
    }

}
