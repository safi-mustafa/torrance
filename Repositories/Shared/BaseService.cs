using AutoMapper;
using DataLibrary;
using Helpers.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Interfaces;
using System.Linq.Expressions;
using ViewModels.Shared;
using Centangle.Common.ResponseHelpers.Models;
using Centangle.Common.ResponseHelpers;
using Helpers.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Common
{
    public abstract class BaseService<TEntity, CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where TEntity : BaseDBModel
        where DetailViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public BaseService(ToranceContext db, ILogger logger, IMapper mapper, IRepositoryResponse response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public virtual async Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            try
            {
                var mappedModel = _mapper.Map<TEntity>(model);
                await _db.Set<TEntity>().AddAsync(mappedModel);
                await _db.SaveChangesAsync();
                var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of {typeof(TEntity).FullName}");
                return Response.BadRequestResponse(_response);
            }
        }

        public virtual async Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            try
            {
                var updateModel = model as BaseUpdateVM;
                if (updateModel != null)
                {
                    var record = await _db.Set<TEntity>().FindAsync(updateModel?.Id);
                    if (record != null)
                    {
                        var dbModel = _mapper.Map(model, record);
                        await _db.SaveChangesAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                        return response;
                    }
                    _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(TEntity).FullName} in Update()");
                }
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for {typeof(TEntity).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public virtual async Task<IRepositoryResponse> Delete(long id)
        {
            try
            {
                var dbModel = await _db.Set<TEntity>().FindAsync(id);
                if (dbModel != null)
                {
                    dbModel.IsDeleted = true;
                    await _db.SaveChangesAsync();
                    return _response;
                }
                _logger.LogWarning($"No record found for id:{id} for {typeof(TEntity).FullName} in Delete()");

                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete() for {typeof(TEntity).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public virtual async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var filters = SetQueryFilter(search);
                var result = await GetPaginationDbSet().Where(filters).Paginate(search);
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();
                    paginatedResult.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    paginatedResult._meta = result._meta;
                    paginatedResult._links = result._links;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(TEntity).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(TEntity).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        public virtual IQueryable<TEntity> GetPaginationDbSet()
        {
            return _db.Set<TEntity>().AsQueryable();
        }
        public virtual async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.Set<TEntity>().FindAsync(id);
                if (dbModel != null)
                {
                    var result = _mapper.Map<DetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<DetailViewModel> { ReturnModel = result };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for {typeof(TEntity).FullName} in GetById()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for {typeof(TEntity).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public virtual Expression<Func<TEntity, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            return p => p.IsDeleted == null || p.IsDeleted == false;
        }
    }
}
