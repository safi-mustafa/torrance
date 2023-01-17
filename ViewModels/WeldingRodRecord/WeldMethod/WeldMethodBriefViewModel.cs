using Select2.Model;
using System.ComponentModel;

namespace ViewModels.WeldingRodRecord.WeldMethod
{
    public class WeldMethodBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public WeldMethodBriefViewModel() : base(true, "The WeldMethod field is required.")
        {

        }
        [DisplayName("WeldMethod")]
        public override string Name { get; set; }
    }

}
