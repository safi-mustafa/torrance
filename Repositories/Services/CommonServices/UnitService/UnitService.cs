using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Common.Contractor;
using ViewModels.Common.Unit;

namespace Repositories.Services.CommonServices.UnitService
{
    public class UnitService : BaseService<Unit, UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel>, IUnitService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<UnitService> _logger;
        private readonly IMapper _mapper;

        public UnitService(ToranceContext db, ILogger<UnitService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<Unit, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as UnitSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
