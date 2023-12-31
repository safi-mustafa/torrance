﻿using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.Common;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using Repositories.Services.CommonServices.ContractorService;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.RodType;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;

namespace Repositories.Services.AppSettingServices.RodTypeService
{
    public class RodTypeService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseServiceWithUniqueNameValidation<RodType, CreateViewModel, UpdateViewModel, DetailViewModel>, IRodTypeService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<RodTypeService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public RodTypeService(ToranceContext db, ILogger<RodTypeService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<RodType, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as RodTypeSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
