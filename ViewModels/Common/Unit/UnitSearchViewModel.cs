using Enums;
using Pagination;

namespace ViewModels.Common.Unit
{
    public class UnitSearchViewModel : BaseSearchModel
    {
        public UnitSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }

        public BaseBriefVM Department { get; set; } = new BaseBriefVM();
        public bool IsSearchForm { get; set; }
        public FilterLogType LogType { get; set; }
    }
}
