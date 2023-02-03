using Enums;
using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.Common.Unit
{
    public class UnitDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Cost Tracker Unit")]
        public string CostTrackerUnit { get; set; }
        public LogType Type { get; set; }
    }
}
