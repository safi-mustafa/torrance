using Select2.Model;
using System.ComponentModel;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ORLogBriefViewModel() : base(true, "The ORLog field is required.")
        {

        }
        [DisplayName("ORLog")]
        public override string Name { get; set; }
    }

}
