using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace Repositories.Services.AppSettingServices.WeldMethodService
{
    public class WeldMethodService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<WeldMethod, CreateViewModel, UpdateViewModel, DetailViewModel>, IWeldMethodService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<WeldMethodService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public WeldMethodService(ToranceContext db, ILogger<WeldMethodService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
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
