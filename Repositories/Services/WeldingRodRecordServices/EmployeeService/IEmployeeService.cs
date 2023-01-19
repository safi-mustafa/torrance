using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.Employee;

namespace Repositories.Services.WeldRodRecordServices.EmployeeService
{
    public interface IEmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<bool> IsEmployeeIdUnique(long id, string employeeId);
        Task<bool> IsEmployeeEmailUnique(long id, string email);
    }
}
