using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.PermitType
{
    public class PermitTypeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public PermitTypeBriefViewModel() : base(true, "The Permit Type field is required.")
        {

        }

        public PermitTypeBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Permit Type field is required.")
        {

        }
        [DisplayName("Permit Type")]
        public override string Name { get; set; }
    }

}
