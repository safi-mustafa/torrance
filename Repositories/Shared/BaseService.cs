using AutoMapper;
using DataLibrary;
using Helpers.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Helpers.Models.Shared;
using Pagination;
using Repositories.Interfaces;
using System.Linq.Expressions;
using ViewModels.Shared;

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

        public BaseService(ToranceContext db, ILogger logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public virtual async Task<long> Create(CreateViewModel model)
        {
            try
            {
                var mappedModel = _mapper.Map<TEntity>(model);
                await _db.Set<TEntity>().AddAsync(mappedModel);
                await _db.SaveChangesAsync();
                return mappedModel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of {typeof(TEntity).FullName}");
                return -1;
            }
        }

        public virtual async Task<long> Update(UpdateViewModel model)
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
                        return record.Id;
                    }
                    _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(TEntity).FullName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for {typeof(TEntity).FullName} threw the following exception");
            }
            return -1;
        }

        public virtual async Task<bool> Delete(long id)
        {
            try
            {
                var dbModel = await _db.Set<TEntity>().FindAsync(id);
                if (dbModel != null)
                {
                    dbModel.IsDeleted = true;
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete() for {typeof(TEntity).FullName} threw the following exception");
            }
            return false;
        }

        public virtual async Task<PaginatedResultModel<M>> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var filters = SetQueryFilter(search);
                var result = await _db.Set<TEntity>().Where(filters).Paginate(search);
                if (result != null)
                {
                    var response = new PaginatedResultModel<M>();
                    response.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    response._meta = result._meta;
                    response._links = result._links;
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(TEntity).FullName} threw an exception.");
            }
            return new PaginatedResultModel<M>();
        }

        public virtual async Task<DetailViewModel> GetById(long id)
        {
            try
            {
                var dbModel = await _db.Set<TEntity>().FindAsync(id);
                if (dbModel != null)
                {
                    return _mapper.Map<DetailViewModel>(dbModel);
                }
                _logger.LogWarning($"No record found for id:{id} for {typeof(TEntity).FullName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for {typeof(TEntity).FullName} threw the following exception");
            }
            return new();
        }

        public virtual Expression<Func<TEntity, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            return p => p.IsDeleted == null || p.IsDeleted == false;
        }
    }
}
