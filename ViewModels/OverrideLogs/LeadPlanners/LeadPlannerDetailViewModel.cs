using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class LeadPlannerDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long Id { get; set; }
        [DisplayName("Lead Planner")]
        public string Name { get; set; }
    }
}
