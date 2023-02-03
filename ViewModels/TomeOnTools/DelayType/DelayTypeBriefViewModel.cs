using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools
{
    public class DelayTypeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public DelayTypeBriefViewModel() : base(true, "The Delay Type field is required.")
        {

        }
        [DisplayName("DelayType")]
        public override string Name { get; set; }
    }

}
