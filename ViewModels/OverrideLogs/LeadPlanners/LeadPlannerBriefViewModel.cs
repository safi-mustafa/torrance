using Select2.Model;
using System.ComponentModel;

namespace ViewModels.OverrideLogs
{
    public class LeadPlannerBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public LeadPlannerBriefViewModel() : base(true, "The Lead Planner field is required.")
        {

        }
        [DisplayName("Lead Planner")]
        public override string Name { get; set; }
    }

}
