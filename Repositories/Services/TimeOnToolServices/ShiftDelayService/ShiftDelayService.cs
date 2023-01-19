using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.TomeOnTools.ShiftDelay;

namespace Repositories.Services.TimeOnToolServices.ShiftDelayService
{
    public class ShiftDelayService : BaseService<ShiftDelay, ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel>, IShiftDelayService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ShiftDelayService> _logger;
        private readonly IMapper _mapper;

        public ShiftDelayService(ToranceContext db, ILogger<ShiftDelayService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<ShiftDelay, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as ShiftDelaySearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
