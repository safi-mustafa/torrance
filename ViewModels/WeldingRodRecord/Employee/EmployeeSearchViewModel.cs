using Enums;
using Pagination;
using ViewModels.Authentication;
using ViewModels.Authentication.User;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeSearchViewModel : UserSearchViewModel
    {
        public bool IsSearchForm { get; set; }

        public bool ShowUnApproved { get; set; }
        public FilterLogType LogType { get; set; }
    }

    public class EmployeeAPISearchViewModel : UserSearchViewModel
    {
    }
}
