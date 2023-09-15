using Models.OverrideLogs;
using Select2.Model;
using System.ComponentModel;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;
using ViewModels.Shared;

namespace ViewModels.Common.Company
{
    public class CompanyDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }

        //public List<BaseBriefVM> Crafts { get; set; } = new List<BaseBriefVM>();
        //public string FormattedCrafts
        //{
        //    get
        //    {
        //        return Crafts != null && Crafts.Count() > 0 ? string.Join(", ", Crafts.Select(m => m.Name).ToList()) : "";
        //    }
        //}
    }
}
