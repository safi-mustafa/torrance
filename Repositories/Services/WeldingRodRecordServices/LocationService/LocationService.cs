using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;
using ViewModels.TomeOnTools.SOW;
using ViewModels.WeldingRodRecord.Location;

namespace Repositories.Services.WeldRodRecordServices.LocationService
{
    public class LocationService : BaseService<Location, LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel>, ILocationService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<LocationService> _logger;
        private readonly IMapper _mapper;

        public LocationService(ToranceContext db, ILogger<LocationService> logger, IMapper mapper) : base(db, logger, mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<Location, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as LocationSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
