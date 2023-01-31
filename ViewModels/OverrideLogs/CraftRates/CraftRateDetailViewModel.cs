using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class CraftRateDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long Id { get; set; }
        [DisplayName("Rate")]
        public float Rate { get; set; }
        public string Name { get => Rate.ToString(); set { } }
    }
}
