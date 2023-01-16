using Pagination;
using Repositories.Interfaces;
using ViewModels.Common.Contractor;

namespace Repositories.Services.CommonServices.ContractorService
{
    public interface IContractorService : IBaseCrud<ContractorModifyViewModel, ContractorModifyViewModel, ContractorDetailViewModel>
    {
    }
}
