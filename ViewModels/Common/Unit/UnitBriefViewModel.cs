using Select2.Model;
using System.ComponentModel;

namespace ViewModels.Common.Unit
{
    public class UnitBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public UnitBriefViewModel() : base(true, "The Unit field is required.")
        {

        }
        [DisplayName("Unit")]
        public override string Name { get; set; }
    }

}
