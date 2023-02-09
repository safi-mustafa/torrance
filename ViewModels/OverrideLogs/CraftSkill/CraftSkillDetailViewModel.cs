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

        [DisplayName("Rate")]
        public double Rate { get; set; }


        [DisplayName("Rate")]
        public string FormattedRate
        {
            get
            {
                return $"${Rate.ToString("N", new CultureInfo("en-US"))}";
            }
        }
    }
}
