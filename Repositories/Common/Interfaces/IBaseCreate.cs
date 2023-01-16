using ViewModels.Shared;

namespace Repositories.Interfaces
{
    public interface IBaseCreate<CreateViewModel>
        where CreateViewModel : class, IBaseCrudViewModel, new()
    {
        Task<long> Create(CreateViewModel model);
    }

}
