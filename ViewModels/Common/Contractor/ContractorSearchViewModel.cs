using Pagination;

namespace ViewModels.Common.Contractor
{
    public class ContractorSearchViewModel : BaseSearchModel
    {
        public string Name { get; set; }
        public override string OrderByColumn { get; set; } = "Id";
    }
}
