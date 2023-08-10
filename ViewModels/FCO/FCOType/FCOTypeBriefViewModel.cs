using Select2.Model;
using System.ComponentModel;

namespace ViewModels
{
    public class FCOTypeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public FCOTypeBriefViewModel() : base(false, "The FCO Type field is required.")
        {

        }
        [DisplayName("FCO Type")]
        public override string Name { get; set; }
    }

}
