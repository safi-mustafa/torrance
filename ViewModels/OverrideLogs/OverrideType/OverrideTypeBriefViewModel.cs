using Select2.Model;
using System.ComponentModel;

namespace ViewModels.OverrideLogs
{
    public class OverrideTypeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public OverrideTypeBriefViewModel() : base(true, "The Override Type field is required.")
        {

        }
        public OverrideTypeBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Override Type field is required.")
        {

        }
        [DisplayName("Override Type")]
        public override string Name { get; set; }
    }

}
