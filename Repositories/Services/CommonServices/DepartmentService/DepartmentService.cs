using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Common.Department;

namespace Repositories.Services.CommonServices.DepartmentService
{
    public class DepartmentService : BaseService<Department, DepartmentModifyViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel>, IDepartmentService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IMapper _mapper;

        public DepartmentService(ToranceContext db, ILogger<DepartmentService> logger, IMapper mapper) : base(db, logger, mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<Department, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as DepartmentSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }
    }
}
