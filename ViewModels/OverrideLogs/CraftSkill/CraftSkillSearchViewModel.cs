using Pagination;
using ViewModels.Common.Company;

namespace ViewModels.OverrideLogs
{
    public class CraftSkillSearchViewModel : BaseSearchModel
    {
        public string Name { get; set; }
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(false);
    }
}
