using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.ReworkDelay
{
    public class ReworkDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ReworkDelayBriefViewModel() : base(false, "")
        {

        }
        [DisplayName("ReworkDelay")]
        public override string? Name { get; set; }
    }

}
