using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.TomeOnTools.TOTLog;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public class TOTLogService : BaseService<TOTLog, TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel>, ITOTLogService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<TOTLogService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public TOTLogService(ToranceContext db, ILogger<TOTLogService> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public override Expression<Func<TOTLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as TOTLogSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.EquipmentNo.ToString().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.EquipmentNo == 0 || x.EquipmentNo == searchFilters.EquipmentNo)
                            &&
                            (string.IsNullOrEmpty(searchFilters.Contractor.Name) || x.Contractor.Name.ToLower().Contains(searchFilters.Contractor.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Department.Name) || x.Department.Name.ToLower().Contains(searchFilters.Department.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Unit.Name) || x.Unit.Name.ToLower().Contains(searchFilters.Unit.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.ReworkDelay.Name) || x.ReworkDelay.Name.ToLower().Contains(searchFilters.ReworkDelay.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.ShiftDelay.Name) || x.ShiftDelay.Name.ToLower().Contains(searchFilters.ShiftDelay.Name.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Shift.Name) || x.Shift.Name.ToLower().Contains(searchFilters.Shift.Name.ToLower()))
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.TOTLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Contractor)
                    .Include(x => x.Approver)
                    .Include(x => x.Foreman)
                    .Include(x => x.ReworkDelay)
                    .Include(x => x.ShiftDelay)
                    .Include(x => x.Shift)
                    .Include(x => x.PermitType)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<TOTLogDetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<TOTLogDetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for TOTLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for TOTLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

    }
}
