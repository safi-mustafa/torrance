using Pagination;
using ViewModels.WeldingRodRecord;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogSearchViewModel : BaseSearchModel
    {
        public EmployeeBriefViewModel Requester { get; set; } = new();
    }

    public class ORLogAPISearchViewModel : BaseSearchModel
    {
        public long RequesterId { get; set; }
    }
}
