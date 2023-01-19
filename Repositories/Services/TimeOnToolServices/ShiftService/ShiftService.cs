using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.TomeOnTools.Shift;

namespace Repositories.Services.TimeOnToolServices.ShiftService
{
    public class ShiftService : BaseService<Shift, ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel>, IShiftService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ShiftService> _logger;
        private readonly IMapper _mapper;

        public ShiftService(ToranceContext db, ILogger<ShiftService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<Shift, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as ShiftSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
