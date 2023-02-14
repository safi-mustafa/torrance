using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels;
using ViewModels.Common.Company;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.MultiSelectInterfaces;
using ViewModels.Shared;

namespace Repositories.Services.CommonServices.DepartmentService
{
    public class DepartmentService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Department, CreateViewModel, UpdateViewModel, DetailViewModel>, IDepartmentService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, IUnitMultiSelect, new()
        where CreateViewModel : class, IBaseCrudViewModel, IUnitMultiSelect, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IUnitMultiSelect, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<DepartmentService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public DepartmentService(ToranceContext db, ILogger<DepartmentService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var mappedModel = _mapper.Map<Department>(model);
                    await _db.Set<Department>().AddAsync(mappedModel);
                    await _db.SaveChangesAsync();

                    var unitMappingResult = await SetDepartmentUnits(model.UnitIds, mappedModel.Id);
                    if (unitMappingResult)
                    {
                        await transaction.CommitAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                        return response;
                    }
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception thrown in Create method of {typeof(Department).FullName}");
                    await transaction.RollbackAsync();
                }
                return Response.BadRequestResponse(_response);
            }
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateModel = model as BaseUpdateVM;
                    if (updateModel != null)
                    {
                        var record = await _db.Set<Department>().FindAsync(updateModel?.Id);
                        if (record != null)
                        {
                            var dbModel = _mapper.Map(model, record);
                            await _db.SaveChangesAsync();
                            var unitMappingResult = await SetDepartmentUnits(model.UnitIds, dbModel.Id);
                            if (unitMappingResult)
                            {
                                await transaction.CommitAsync();
                                var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                                return response;
                            }
                            await transaction.RollbackAsync();
                        }
                        _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(Department).FullName} in Update()");
                    }
                    return Response.NotFoundResponse(_response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Update() for {typeof(Department).FullName} threw the following exception");
                    return Response.BadRequestResponse(_response);
                }
            }
        }

        public async override Task<IRepositoryResponse> GetById(long id)
        {
            var department = await base.GetById(id);
            var response = department as RepositoryResponseWithModel<DetailViewModel>;
            if (response?.ReturnModel != null)
            {
                response.ReturnModel.Units = await GetDepartmentUnits(id);
            }
            return response;
        }

        public override async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var searchFilter = search as DepartmentSearchViewModel;

                searchFilter.OrderByColumn = string.IsNullOrEmpty(search.OrderByColumn) ? "Id" : search.OrderByColumn;

                var departmentsQueryable = (from c in _db.Departments
                                            join du in _db.DepartmentUnits on c.Id equals du.DepartmentId into dul
                                            from du in dul.DefaultIfEmpty()
                                            where
                                            (
                                               (
                                                   string.IsNullOrEmpty(searchFilter.Search.value) || c.Name.ToLower().Contains(searchFilter.Search.value.ToLower())
                                               )
                                               &&
                                               (searchFilter.Unit.Id == null || searchFilter.Unit.Id == 0 || du.UnitId == searchFilter.Unit.Id)
                                               &&
                                               (
                                                   string.IsNullOrEmpty(searchFilter.Name) || c.Name.ToLower().Contains(searchFilter.Name.ToLower())
                                               )
                                           )
                                            select c
                            ).GroupBy(x => x.Id)
                            .Select(x => new DepartmentDetailViewModel { Id = x.Key, Name = x.Max(m => m.Name) })
                            .AsQueryable();
                var paginatedDepartments = await departmentsQueryable.Paginate(searchFilter);
                var filteredDepartmentIds = paginatedDepartments.Items.Select(x => x.Id);


                var departmentUnits = await _db.DepartmentUnits.Include(x => x.Unit).Where(x => filteredDepartmentIds.Contains(x.DepartmentId)).ToListAsync();

                paginatedDepartments.Items.ForEach(x =>
                {
                    x.Units = _mapper.Map<List<UnitBriefViewModel>>(departmentUnits.Where(u => u.DepartmentId == x.Id).Select(x => x.Unit).ToList());
                });
                var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<M>>();
                responseModel.ReturnModel = paginatedDepartments as PaginatedResultModel<M>;
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Department Service GetAll method threw an exception, Message: {ex.Message}");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<bool> SetDepartmentUnits(List<long> unitIds, long departmentId)
        {
            try
            {
                var oldDepartmentUnits = await _db.DepartmentUnits.Where(x => x.DepartmentId == departmentId).ToListAsync();
                if (oldDepartmentUnits.Count() > 0)
                {
                    foreach (var oldDepartmentUnit in oldDepartmentUnits)
                    {
                        oldDepartmentUnit.IsDeleted = true;
                        _db.Entry(oldDepartmentUnit).State = EntityState.Modified;
                    }
                    _db.SaveChanges();
                }
                if (unitIds.Count() > 0)
                {
                    List<DepartmentUnit> list = new List<DepartmentUnit>();
                    foreach (var unitId in unitIds)
                    {
                        DepartmentUnit departmentUnit = new();
                        departmentUnit.DepartmentId = departmentId;
                        departmentUnit.UnitId = unitId;
                        list.Add(departmentUnit);
                    }
                    await _db.AddRangeAsync(list);
                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"DepartmentService SetDepartmentUnits method threw an exception, Message: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UnitBriefViewModel>> GetDepartmentUnits(long id)
        {
            try
            {
                var departmentUnits = await (from au in _db.DepartmentUnits
                                             where au.DepartmentId == id
                                             join u in _db.Units on au.UnitId equals u.Id
                                             select new UnitBriefViewModel()
                                             {
                                                 Id = au.UnitId,
                                                 Name = u.Name
                                             }).ToListAsync();
                return departmentUnits;
            }
            catch (Exception ex)
            {
                _logger.LogError($"DepartmentService GetDepartmentUnits method threw an exception, Message: {ex.Message}");
                return null;
            }
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
