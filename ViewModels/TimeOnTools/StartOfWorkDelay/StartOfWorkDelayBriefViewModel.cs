using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.StartOfWorkDelay
{
    public class StartOfWorkDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public StartOfWorkDelayBriefViewModel() : base(false, "")
        {

        }
        [DisplayName("ShiftDelay")]
        public override string? Name { get; set; }
    }

}
