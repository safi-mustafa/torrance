using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.WeldRodRecordServices.WRRLogService
{
    public class WRRLogService : BaseService<WRRLog, WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel>, IWRRLogService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<WRRLogService> _logger;
        private readonly IMapper _mapper;

        public WRRLogService(ToranceContext db, ILogger<WRRLogService> logger, IMapper mapper) : base(db, logger, mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<WRRLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as WRRLogSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Email.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Email) || x.Email.ToLower().Contains(searchFilters.Email.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Employee.Name) || x.Email.ToLower().Contains(searchFilters.Employee.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Department.Name) || x.Email.ToLower().Contains(searchFilters.Department.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Unit.Name) || x.Email.ToLower().Contains(searchFilters.Unit.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.RodType.Name) || x.Email.ToLower().Contains(searchFilters.RodType.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.WeldMethod.Name) || x.Email.ToLower().Contains(searchFilters.WeldMethod.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Location.Name) || x.Email.ToLower().Contains(searchFilters.Location.Name.ToLower()))
                        ;
        }
    }
}
