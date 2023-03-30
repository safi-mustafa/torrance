using Pagination;
using ViewModels.Common.Company;

namespace ViewModels.OverrideLogs
{
    public class CraftSkillSearchViewModel : BaseSearchModel
    {
        public CraftSkillSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(false,"");
    }
}
