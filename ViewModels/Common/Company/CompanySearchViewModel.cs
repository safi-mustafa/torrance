using Pagination;

namespace ViewModels.Common.Company
{
    public class CompanySearchViewModel : BaseSearchModel
    {
        public string Name { get; set; }
        public override string OrderByColumn { get; set; } = "Id";
    }
}
