using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.ShiftDelay
{
    public class ShiftDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ShiftDelayBriefViewModel() : base(false, "The Shift Delay field is required.")
        {

        }
        [DisplayName("ShiftDelay")]
        public override string? Name { get; set; }
    }

}
