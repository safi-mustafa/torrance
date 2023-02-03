using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public TOTLogBriefViewModel() : base(true, "The TOTLog field is required.")
        {

        }
        [DisplayName("TOTLog")]
        public override string Name { get; set; }
    }

}
