using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.WeldingRodRecord.Employee;

namespace Repositories.Services.WeldRodRecordServices.EmployeeService
{
    public class EmployeeService : BaseService<Employee, EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel>, IEmployeeService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(ToranceContext db, ILogger<EmployeeService> logger, IMapper mapper) : base(db, logger, mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<Employee, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as EmployeeSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.FirstName.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.FirstName) || x.FirstName.ToLower().Contains(searchFilters.FirstName.ToLower()))
                        ;
        }
    }
}
