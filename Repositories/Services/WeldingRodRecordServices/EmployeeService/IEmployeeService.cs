using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Authentication;
using ViewModels.Shared;

namespace Repositories.Services.WeldRodRecordServices.EmployeeService
{
    public interface IEmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<bool> IsEmployeeIdUnique(long id, string employeeId);
        Task<bool> IsEmployeeEmailUnique(long id, string email);
        Task<IRepositoryResponse> ResetAccessCode(ChangeAccessCodeVM model);
    }
}
