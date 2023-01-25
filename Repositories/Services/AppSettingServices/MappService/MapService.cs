using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.AppSettings;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.AppSettings.Map;
using ViewModels.Shared;

namespace Repositories.Services.AppSettingServices.MappService
{
    public class MapService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Map, CreateViewModel, UpdateViewModel, DetailViewModel>, IMapService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<MapService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public MapService(ToranceContext db, ILogger<MapService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public override Expression<Func<Map, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as MapSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Latitude.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Latitude) || x.Latitude.ToLower().Contains(searchFilters.Latitude.ToLower()))
                        ;
        }
    }
}
