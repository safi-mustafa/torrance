using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.TomeOnTools.SOW;

namespace Repositories.Services.TimeOnToolServices.SOWService
{
    public class SOWService : BaseService<StatementOfWork, SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel>, ISOWService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<SOWService> _logger;
        private readonly IMapper _mapper;

        public SOWService(ToranceContext db, ILogger<SOWService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
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
