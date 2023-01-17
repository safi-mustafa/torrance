using Select2.Model;
using System.ComponentModel;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public WRRLogBriefViewModel() : base(true, "The WRRLog field is required.")
        {

        }
        [DisplayName("WRRLog")]
        public override string Name { get; set; }
    }

}
