using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.ShiftDelay
{
    public class ShiftDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ShiftDelayBriefViewModel() : base(false, "The field Shift Delay is required.")
        {

        }
        [DisplayName("Shift Delay")]
        public override string? Name { get; set; }
    }

}
