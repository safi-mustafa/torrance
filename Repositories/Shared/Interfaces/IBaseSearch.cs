using Pagination;

namespace Repositories.Interfaces
{
    public interface IBaseSearch
    {
        Task<PaginatedResultModel<M>> GetAll<M>(IBaseSearchModel model);
    }

}
