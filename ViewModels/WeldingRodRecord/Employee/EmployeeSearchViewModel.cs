using Enums;
using Pagination;
using ViewModels.Authentication;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeSearchViewModel : BaseSearchModel
    {
    //    public ActiveStatus? Status { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }
    }

    public class EmployeeAPISearchViewModel : BaseSearchModel
    {
      //  public ActiveStatus? Status { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }
    }
}
