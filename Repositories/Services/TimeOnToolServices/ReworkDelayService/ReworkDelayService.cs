using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.TomeOnTools.ReworkDelay;

namespace Repositories.Services.TimeOnToolServices.ReworkService
{
    public class ReworkDelayService : BaseService<ReworkDelay, ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel>, IReworkDelayService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ReworkDelayService> _logger;
        private readonly IMapper _mapper;

        public ReworkDelayService(ToranceContext db, ILogger<ReworkDelayService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<ReworkDelay, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as ReworkDelaySearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
