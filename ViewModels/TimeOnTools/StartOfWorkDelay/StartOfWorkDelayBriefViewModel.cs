using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.StartOfWorkDelay
{
    public class StartOfWorkDelayBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public StartOfWorkDelayBriefViewModel() : base(false, "The field Start Of Work is required.")
        {

        }
        [DisplayName("Start Of Work Delay")]
        public override string? Name { get; set; }
    }

}
