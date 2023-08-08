using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Pagination;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;
using Models.WeldingRodRecord;

namespace Repositories.Services.AppSettingServices.FCOTypeService
{
    public class FCOTypeService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseServiceWithUniqueNameValidation<FCOType, CreateViewModel, UpdateViewModel, DetailViewModel>, IFCOTypeService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<FCOTypeService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public FCOTypeService(ToranceContext db, ILogger<FCOTypeService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<FCOType, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as FCOTypeSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
