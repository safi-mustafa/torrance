using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Common.Contractor;

namespace Repositories.Services.CommonServices.ContractorService
{
    public class ContractorService : BaseService<Contractor, ContractorModifyViewModel, ContractorModifyViewModel, ContractorDetailViewModel>, IContractorService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ContractorService> _logger;
        private readonly IMapper _mapper;

        public ContractorService(ToranceContext db, ILogger<ContractorService> logger, IMapper mapper) : base(db, logger, mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<Contractor, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as ContractorSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
