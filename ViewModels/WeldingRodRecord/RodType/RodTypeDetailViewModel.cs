using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.WeldingRodRecord.RodType
{
    public class RodTypeDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
