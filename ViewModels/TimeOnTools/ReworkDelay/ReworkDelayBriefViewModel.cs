using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.ReworkDelay
{
    public class ReworkDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ReworkDelayBriefViewModel() : base(false, "")
        {

        }
        [DisplayName("Rework Delay")]
        public override string? Name { get; set; }
    }

}
