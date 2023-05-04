using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Common;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;
using System.Linq.Expressions;
using ViewModels.Authentication.Approver;
using ViewModels.Common.Contractor;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;
using ViewModels.Shared;
using System.Linq.Dynamic.Core;
using ViewModels.WeldingRodRecord.Employee;

namespace Repositories.Services.CommonServices.UnitService
{
    public class UnitService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseServiceWithUniqueNameValidation<Unit, CreateViewModel, UpdateViewModel, DetailViewModel>, IUnitService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<UnitService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public UnitService(ToranceContext db, ILogger<UnitService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public override Expression<Func<Unit, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as UnitSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }

        public async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {

            try
            {
                var searchFilters = search as UnitSearchViewModel;

                searchFilters.OrderByColumn = string.IsNullOrEmpty(search.OrderByColumn) ? "Id" : search.OrderByColumn;

                var unitQueryable = GetPaginationDbSet(searchFilters);

                var units = await unitQueryable.Paginate(searchFilters);
                var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<UnitDetailViewModel>>();
                responseModel.ReturnModel = units;
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unit Service GetAll method threw an exception, Message: {ex.Message}");
                return Response.BadRequestResponse(_response);
            }
        }
        public IQueryable<UnitDetailViewModel> GetPaginationDbSet(UnitSearchViewModel search)
        {
            var unitQueryable = (from u in _db.Units
                                 join du in _db.DepartmentUnits on u.Id equals du.UnitId into dul
                                 from du in dul.DefaultIfEmpty()
                                 where
                                 (
                                     (string.IsNullOrEmpty(search.Search.value) || u.Name.ToLower().Contains(search.Search.value.ToLower()))
                                     &&
                                     (string.IsNullOrEmpty(search.Name) || u.Name.ToLower().Contains(search.Name.ToLower()))
                                     &&
                                     (
                                        search.Department.Id == null || search.Department.Id == 0 || search.Department.Id == du.DepartmentId
                                     )
                                )
                                 select u);
            //select new ApproverDetailViewModel { Id = user.Id });
            if (search.IsSearchForm && search.LogType != FilterLogType.None)
            {
                switch (search.LogType)
                {
                    case FilterLogType.Override:
                        unitQueryable = JoinUnitsWithLogs<OverrideLog>(unitQueryable);
                        break;
                    case FilterLogType.TimeOnTools:
                        unitQueryable = JoinUnitsWithLogs<TOTLog>(unitQueryable);
                        break;
                    case FilterLogType.WeldingRodRecord:
                        unitQueryable = JoinUnitsWithLogs<WRRLog>(unitQueryable);
                        break;
                    case FilterLogType.All:
                        return (
                             from u in unitQueryable
                             join tl in _db.TOTLogs on u.Id equals tl.UnitId into ttl
                             from tl in ttl.DefaultIfEmpty()
                             join wl in _db.WRRLogs on u.Id equals wl.UnitId into wwl
                             from wl in wwl.DefaultIfEmpty()
                             join ol in _db.OverrideLogs on u.Id equals ol.UnitId into ool
                             from ol in ool.DefaultIfEmpty()
                             where
                                 (tl != null && tl.Status != Status.Approved)
                                 ||
                                 (wl != null && wl.Status != Status.Approved)
                                 ||
                                 (ol != null && ol.Status != Status.Approved)
                             group u by u.Id
                            ).Select(x => new UnitDetailViewModel
                            {
                                Id = x.Key,
                                Name = x.Max(m => m.Name),
                                CostTrackerUnit = x.Max(m => m.CostTrackerUnit),
                            });
                }
            }
            return unitQueryable.OrderColumns(search).GroupBy(x => x.Id)
                            .Select(x => new UnitDetailViewModel
                            {
                                Id = x.Key,
                                Name = x.Max(m => m.Name),
                                CostTrackerUnit = x.Max(m => m.CostTrackerUnit),
                            })
                            .AsQueryable();
        }
        private IQueryable<Unit> JoinUnitsWithLogs<T>(IQueryable<Unit> unitQueryable, bool isInnerJoin = false) where T : class, IBaseModel, IUnitId, IApprove
        {
            if (isInnerJoin == false)
                return unitQueryable.Join(_db.Set<T>(), ol => ol.Id, u => u.UnitId, (u, ol) => new { u, ol }).Where(x => x.ol.Status != Status.Pending).Select(x => x.u);
            else
                return unitQueryable.GroupJoin(_db.Set<T>(), ol => ol.Id, u => u.UnitId, (u, ols) => new { u, ols })
                    .SelectMany(x => x.ols.DefaultIfEmpty(), (u, ol) => new { u = u.u, ol = ol })
                    .Where(x => x.ol != null)
                    .Select(x => x.u);
        }

    }
}
