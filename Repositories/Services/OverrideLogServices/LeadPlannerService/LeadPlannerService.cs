using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Shared;
using Models.OverrideLogs;
using ViewModels.OverrideLogs;

namespace Repositories.Services.OverrideLogServices.LeadPlannerService
{
    public class LeadPlannerService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<LeadPlanner, CreateViewModel, UpdateViewModel, DetailViewModel>, ILeadPlannerService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<LeadPlannerService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;

        public LeadPlannerService(ToranceContext db, ILogger<LeadPlannerService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public override Expression<Func<LeadPlanner, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as LeadPlannerSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Email.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Email) || x.Email.ToLower().Contains(searchFilters.Email.ToLower()))
                        ;
        }
    }
}
