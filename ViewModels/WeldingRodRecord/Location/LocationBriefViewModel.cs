using Select2.Model;
using System.ComponentModel;

namespace ViewModels.WeldingRodRecord.Location
{
    public class LocationBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public LocationBriefViewModel() : base(true, "The Location field is required.")
        {

        }
        [DisplayName("Location")]
        public override string Name { get; set; }
    }

}
