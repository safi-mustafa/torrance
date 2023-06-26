using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.TimeOnTools.OngoingWorkDelay
{
    public class OngoingWorkDelayDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
