using Pagination;
using Repositories.Interfaces;
using ViewModels.Common.Department;

namespace Repositories.Services.CommonServices.DepartmentService
{
    public interface IDepartmentService : IBaseCrud<DepartmentModifyViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel>
    {
    }
}
