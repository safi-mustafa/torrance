using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common;
using Models.Common.Interfaces;
using Repositories.Common;
using ViewModels.Common.Validation;
using ViewModels.Shared;

namespace Repositories.Services.CommonServices.ValidationService.UniqueNameService
{
    public class BaseServiceWithUniqueNameValidation<TEntity, CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<TEntity, CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseServiceWithUniqueNameValidation
        where TEntity : class, IBaseModel, IName, IIsDeleted
        where DetailViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        public BaseServiceWithUniqueNameValidation(ToranceContext db, ILogger logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
        }
        public async Task<bool> IsUniqueName(IValidateName validateName)
        {
            return ((await _db.Set<TEntity>().Where(x => x.Name == validateName.Name && x.Id != validateName.Id && x.IsDeleted == false).CountAsync()) < 1);
        }
    }
}
