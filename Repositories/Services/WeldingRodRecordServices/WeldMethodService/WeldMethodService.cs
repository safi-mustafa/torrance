using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace Repositories.Services.WeldRodRecordServices.WeldMethodService
{
    public class WeldMethodService : BaseService<WeldMethod, WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel>, IWeldMethodService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<WeldMethodService> _logger;
        private readonly IMapper _mapper;

        public WeldMethodService(ToranceContext db, ILogger<WeldMethodService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<WeldMethod, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as WeldMethodSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
