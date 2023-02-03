using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ViewModels.OverrideLogs
{
    public class LeadPlannerDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Lead Planner Email")]
        public string Email { get; set; }
        public string Name { get => Email; set { } }
    }
}
