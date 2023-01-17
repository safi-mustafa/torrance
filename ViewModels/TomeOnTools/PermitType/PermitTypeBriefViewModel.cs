using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TomeOnTools.PermitType
{
    public class PermitTypeBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public PermitTypeBriefViewModel() : base(true, "The Permit Type field is required.")
        {

        }
        [DisplayName("PermitType")]
        public override string Name { get; set; }
    }

}
