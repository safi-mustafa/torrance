using Models.OverrideLogs;
using Pagination;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;

namespace ViewModels.Common.Company
{
    public class CompanySearchViewModel : BaseSearchModel
    {
        public string Name { get; set; }
        public override string OrderByColumn { get; set; } = "Id";

        public CraftSkillBriefViewModel CraftSkill { get; set; } = new();
    }
}
