using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;
using System.Linq.Expressions;
using ViewModels.Common.Contractor;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;
using ViewModels.Shared;

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

                var unitQueryable = (from u in _db.Units
                                            join du in _db.DepartmentUnits on u.Id equals du.UnitId into dul
                                            from du in dul.DefaultIfEmpty()
                                            where
                                            (
                                                (string.IsNullOrEmpty(searchFilters.Search.value) || u.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                                                &&
                                                (string.IsNullOrEmpty(searchFilters.Name) || u.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                                                &&
                                                (
                                                   searchFilters.Department.Id == null || searchFilters.Department.Id == 0 || searchFilters.Department.Id == du.DepartmentId
                                                )
                                           )
                                            select u
                            ).GroupBy(x => x.Id)
                            .Select(x => new UnitDetailViewModel
                            {
                                Id = x.Key,
                                Name = x.Max(m => m.Name),
                                CostTrackerUnit = x.Max(m => m.CostTrackerUnit),
                            })
                            .AsQueryable();


                var crafts = await unitQueryable.Paginate(searchFilters);
                var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<UnitDetailViewModel>>();
                responseModel.ReturnModel = crafts;
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unit Service GetAll method threw an exception, Message: {ex.Message}");
                return Response.BadRequestResponse(_response);
            }
        }
    }
}
