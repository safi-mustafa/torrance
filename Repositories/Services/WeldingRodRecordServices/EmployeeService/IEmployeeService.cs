using Repositories.Interfaces;
using ViewModels.WeldingRodRecord.Employee;

namespace Repositories.Services.WeldRodRecordServices.EmployeeService
{
    public interface IEmployeeService : IBaseCrud<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel>
    {
    }
}
