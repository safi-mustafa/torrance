using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TomeOnTools.Shift
{
    public class ShiftBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public ShiftBriefViewModel() : base(true, "The Shift field is required.")
        {

        }
        [DisplayName("Shift")]
        public override string Name { get; set; }
    }

}
