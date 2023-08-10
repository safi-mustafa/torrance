using Select2.Model;
using System.ComponentModel;

namespace ViewModels
{
    public class FCOLogBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public FCOLogBriefViewModel() : base(false, "The FCO Log field is required.")
        {

        }
        [DisplayName("FCO Log")]
        public override string? Name { get; set; }
    }

}
