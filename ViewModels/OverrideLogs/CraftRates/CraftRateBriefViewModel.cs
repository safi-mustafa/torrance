using Select2.Model;
using System.ComponentModel;

namespace ViewModels.OverrideLogs
{
    public class CraftRateBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public CraftRateBriefViewModel() : base(true, "The Craft Rate field is required.")
        {

        }
        [DisplayName("Craft Rate")]
        public override string Name { get; set; }
    }

}
