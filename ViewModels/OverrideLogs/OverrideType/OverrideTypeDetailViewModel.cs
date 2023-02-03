using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class OverrideTypeDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Override Type")]
        public string Name { get; set; }
    }
}
