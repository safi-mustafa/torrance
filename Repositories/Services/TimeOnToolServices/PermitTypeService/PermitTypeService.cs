using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.TomeOnTools.PermitType;

namespace Repositories.Services.TimeOnToolServices.PermitTypeService
{
    public class PermitTypeService : BaseService<PermitType, PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel>, IPermitTypeService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<PermitTypeService> _logger;
        private readonly IMapper _mapper;

        public PermitTypeService(ToranceContext db, ILogger<PermitTypeService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<PermitType, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as PermitTypeSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
