using Select2.Model;
using System.ComponentModel;

namespace ViewModels.TimeOnTools.PermittingIssue
{
    public class PermittingIssueBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public PermittingIssueBriefViewModel() : base(true, "The Permitting Issue field is required.")
        {

        }
        [DisplayName("Permitting Issue")]
        public override string Name { get; set; }
    }

}
