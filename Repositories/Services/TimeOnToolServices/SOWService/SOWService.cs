using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.TimeOnTools.SOW;

namespace Repositories.Services.TimeOnToolServices.SOWService
{
    public class SOWService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<StatementOfWork, CreateViewModel, UpdateViewModel, DetailViewModel>, ISOWService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<SOWService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public SOWService(ToranceContext db, ILogger<SOWService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<StatementOfWork, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as SOWSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
