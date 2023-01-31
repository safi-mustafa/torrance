using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Repositories.Common;
using ViewModels.Shared;
using Models.OverrideLogs;
using ViewModels.OverrideLogs;
using System.Linq.Expressions;
using Pagination;

namespace Repositories.Services.OverrideLogServices.CraftRateService
{
    public class CraftRateService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<CraftRate, CreateViewModel, UpdateViewModel, DetailViewModel>, ICraftRateService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<CraftRateService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public CraftRateService(ToranceContext db, ILogger<CraftRateService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<CraftRate, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as CraftRateSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Rate.ToString().ToLower().Contains(searchFilters.Search.value.ToLower()))
                        ;
        }
    }
}
