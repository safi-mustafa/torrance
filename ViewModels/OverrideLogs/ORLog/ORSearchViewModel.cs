using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ViewModels.WeldingRodRecord;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogSearchViewModel : BaseSearchModel
    {
        public EmployeeBriefViewModel Employee { get; set; } = new();

        public StatusSearchEnum? Status { get; set; } = null;
        [Display(Name = "Status Is Not")]
        public Status? StatusNot { get; set; } = null;
    }

    public class ORLogAPISearchViewModel : BaseSearchModel
    {
        public long RequesterId { get; set; }
    }
}
