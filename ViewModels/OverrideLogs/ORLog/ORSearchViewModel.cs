using Enums;
using Pagination;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogSearchViewModel : BaseSearchModel
    {
        public string RequesterEmail { get; set; }
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
    }

    public class ORLogAPISearchViewModel : BaseSearchModel
    {
        public long RequesterEmail { get; set; }
        public long ContractorId { get; set; }
    }
}
