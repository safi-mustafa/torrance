using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class ReasonForRequestDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Reason for Request")]
        public string Name { get; set; }
    }
}
