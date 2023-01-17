using Select2.Model;
using System.ComponentModel;

namespace ViewModels.WeldingRodRecord.RodType
{
    public class RodTypeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public RodTypeBriefViewModel() : base(true, "The RodType field is required.")
        {

        }
        [DisplayName("RodType")]
        public override string Name { get; set; }
    }

}
