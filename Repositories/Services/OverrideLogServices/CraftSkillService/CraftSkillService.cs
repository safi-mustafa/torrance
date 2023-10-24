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
using Helpers.Extensions;
using Centangle.Common.ResponseHelpers;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;

namespace Repositories.Services.OverrideLogServices.CraftSkillService
{
    public class CraftSkillService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseServiceWithUniqueNameValidation<CraftSkill, CreateViewModel, UpdateViewModel, DetailViewModel>, ICraftSkillService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<CraftSkillService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public CraftSkillService(ToranceContext db, ILogger<CraftSkillService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public override Expression<Func<CraftSkill, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as CraftSkillSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()));
        }

        public async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var searchFilters = search as CraftSkillSearchViewModel;

                searchFilters.OrderByColumn = string.IsNullOrEmpty(search.OrderByColumn) ? "Id" : search.OrderByColumn;

                var craftSkillsQueryable = (from cs in _db.CraftSkills
                                            join cc in _db.CompanyCrafts on cs.Id equals cc.CraftSkillId into ccl
                                            from cc in ccl.DefaultIfEmpty()
                                            where
                                            (
                                                (string.IsNullOrEmpty(searchFilters.Search.value) || cs.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                                                &&
                                                (string.IsNullOrEmpty(searchFilters.Name) || cs.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                                           //&&
                                           //(
                                           //   searchFilters.Company.Id == null || searchFilters.Company.Id == 0 || searchFilters.Company.Id == cc.CompanyId
                                           //)
                                           )
                                            select cs
                            ).GroupBy(x => x.Id)
                            .Select(x => new CraftSkillDetailViewModel
                            {
                                Id = x.Key,
                                Name = x.Max(m => m.Name),
                                STRate = x.Max(m => m.STRate),
                                OTRate = x.Max(m => m.OTRate),
                                DTRate = x.Max(m => m.DTRate)
                            })
                            .AsQueryable();


                var crafts = await craftSkillsQueryable.Paginate(searchFilters);
                var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<CraftSkillDetailViewModel>>();
                responseModel.ReturnModel = crafts;
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CraftSkillService GetAll method threw an exception, Message: {ex.Message}");
                return Response.BadRequestResponse(_response);
            }
        }
    }
}
