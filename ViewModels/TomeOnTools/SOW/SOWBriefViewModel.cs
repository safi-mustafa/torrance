using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.SOW
{
    public class SOWBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public SOWBriefViewModel() : base(true, "The SOW field is required.")
        {

        }
        [DisplayName("SOW")]
        public override string Name { get; set; }
    }

}
