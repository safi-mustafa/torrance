﻿using AutoMapper;
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
using ViewModels.TimeOnTools.Shift;
using ViewModels.AppSettings.Map;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;

namespace Repositories.Services.TimeOnToolServices.ShiftService
{
    public class ShiftService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseServiceWithUniqueNameValidation<Shift, CreateViewModel, UpdateViewModel, DetailViewModel>, IShiftService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<ShiftService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public ShiftService(ToranceContext db, ILogger<ShiftService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
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
