using AutoMapper;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Common.Contractor;
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

        public override async Task<WRRLogDetailViewModel> GetById(long id)
        {
            try
            {
                var dbModel = await _db.WRRLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Location)
                    .Include(x => x.Employee)
                    .Include(x => x.RodType)
                    .Include(x => x.WeldMethod)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<WRRLogDetailViewModel>(dbModel);
                    return mappedModel;
                }
                _logger.LogWarning($"No record found for id:{id} for WRRLog");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for WRRLog threw the following exception");
            }
            return new();
        }

        public async Task<bool> IsWRRLogEmailUnique(int id, string email)
        {
            var check = await _db.WRRLogs.Where(x => x.Email == email && x.Id != id).CountAsync();
            return check < 1;
        }
    }
}
