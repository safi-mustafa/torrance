using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.ReworkDelay
{
    public class ReworkDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ReworkDelayBriefViewModel() : base(true, "The Rework Delay field is required.")
        {

        }
        [DisplayName("ReworkDelay")]
        public override string Name { get; set; }
    }

}
