using Enums;
using Models.OverrideLogs;
using Pagination;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;

namespace ViewModels.Common.Company
{
    public class CompanySearchViewModel : BaseSearchModel
    {
        public CompanySearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
        public CraftSkillBriefViewModel CraftSkill { get; set; } = new();
        public bool IsSearchForm { get; set; }
        public FilterLogType LogType { get; set; }
    }
}
