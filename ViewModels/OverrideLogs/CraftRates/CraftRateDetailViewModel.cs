using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class CraftRateDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
