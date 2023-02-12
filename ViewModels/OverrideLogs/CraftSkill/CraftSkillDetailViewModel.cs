using Select2.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class CraftSkillDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Skill")]
        public string Name { get; set; }

        [DisplayName("ST Rate")]
        public double STRate { get; set; }

        [DisplayName("OT Rate")]
        public double OTRate { get; set; }

        [DisplayName("DT Rate")]
        public double DTRate { get; set; }
    }
}
