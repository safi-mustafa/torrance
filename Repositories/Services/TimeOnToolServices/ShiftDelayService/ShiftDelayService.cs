using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.Common;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using Repositories.Services.CommonServices.ContractorService;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.TomeOnTools.ShiftDelay;

namespace Repositories.Services.TimeOnToolServices.ShiftDelayService
{
    public class ShiftDelayService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<ShiftDelay, CreateViewModel, UpdateViewModel, DetailViewModel>, IShiftDelayService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ShiftDelayService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public ShiftDelayService(ToranceContext db, ILogger<ShiftDelayService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
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
