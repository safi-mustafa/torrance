using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.WeldingRodRecord.RodType;

namespace Repositories.Services.WeldRodRecordServices.RodTypeService
{
    public class RodTypeService : BaseService<RodType, RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel>, IRodTypeService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<RodTypeService> _logger;
        private readonly IMapper _mapper;

        public RodTypeService(ToranceContext db, ILogger<RodTypeService> logger, IMapper mapper) : base(db, logger, mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<RodType, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as RodTypeSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
