using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.ShiftDelay
{
    public class ShiftDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ShiftDelayBriefViewModel() : base(false, "")
        {

        }
        [DisplayName("Shift Delay")]
        public override string? Name { get; set; }
    }

}
