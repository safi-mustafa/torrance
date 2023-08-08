using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels.Common.Contractor
{
    public class ContractorDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Composite Rate")]
        public double CompositeRate { get; set; }
    }
}
