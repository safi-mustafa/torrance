using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools
{
    public class DelayTypeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public DelayTypeBriefViewModel() : base(true, "The Delay Type field is required.")
        {

        }

        public DelayTypeBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Delay Type field is required.")
        {

        }
        public DelayTypeBriefViewModel(bool isValidationEnabled, string errorMessage) : base(isValidationEnabled, errorMessage)
        {

        }
        [DisplayName("DelayType")]
        public override string? Name { get; set; }
        public string? Identifier { get; set; }
    }

}
