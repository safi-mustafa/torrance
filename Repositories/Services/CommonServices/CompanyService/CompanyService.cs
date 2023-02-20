using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using System.Data;
using System.Linq.Expressions;
using System.Security.Principal;
using ViewModels.Authentication.User;
using ViewModels.Authentication;
using ViewModels.Common.Company;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using Helpers.Extensions;
using Centangle.Common.ResponseHelpers;
using ViewModels.OverrideLogs;
using ViewModels.Authentication.Approver;
using ViewModels;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;

namespace Repositories.Services.CommonServices.CompanyService
{
    public class CompanyService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseServiceWithUniqueNameValidation<Company, CreateViewModel, UpdateViewModel, DetailViewModel>, ICompanyService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<CompanyService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public CompanyService(ToranceContext db, ILogger<CompanyService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public override Expression<Func<Company, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as CompanySearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }

        public override async Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as CompanyModifyViewModel;
                var company = _mapper.Map<Company>(model);
                _db.Companies.Add(company);
                await _db.SaveChangesAsync();
                if (company.Id > 0)
                {
                    var result = await SetCompanyCraftSkills(viewModel.CraftIds, company.Id);
                    if (result)
                    {
                        await transaction.CommitAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = company.Id };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of User Creation ");
            }
            await transaction.RollbackAsync();
            return Response.BadRequestResponse(_response);
        }

        public override async Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var viewModel = model as CompanyModifyViewModel;
                if (model != null)
                {
                    var company = await _db.Companies.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (company != null)
                    {
                        company.Name = viewModel.Name;
                        await _db.SaveChangesAsync();
                        var mappingsModified = await SetCompanyCraftSkills(viewModel.CraftIds, company.Id);
                        if (mappingsModified)
                        {
                            await transaction.CommitAsync();
                            var response = new RepositoryResponseWithModel<long> { ReturnModel = company.Id };
                            return response;
                        }
                    }
                    _logger.LogWarning($"Record for id: {model?.Id} not found in Employee");
                    await transaction.RollbackAsync();
                    return Response.NotFoundResponse(_response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for Employee threw the following exception");
            }
            await transaction.RollbackAsync();
            return Response.BadRequestResponse(_response);
        }


        public override async Task<IRepositoryResponse> GetById(long id)
        {
            var response = await base.GetById(id);
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                var companyModel = (response as RepositoryResponseWithModel<CompanyDetailViewModel>).ReturnModel;
                companyModel.Crafts = await GetCompanyCraftSkills(companyModel.Id ?? 0);
            }
            return response;
        }
        public override async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var searchFilter = search as CompanySearchViewModel;

                searchFilter.OrderByColumn = string.IsNullOrEmpty(search.OrderByColumn) ? "Id" : search.OrderByColumn;

                var companiesQueryable = (from c in _db.Companies
                                          join cc in _db.CompanyCrafts on c.Id equals cc.CompanyId into ccul
                                          from cc in ccul.DefaultIfEmpty()
                                          where
                                          (
                                             (
                                                 string.IsNullOrEmpty(searchFilter.Search.value) || c.Name.ToLower().Contains(searchFilter.Search.value.ToLower())
                                             )
                                             &&
                                             (searchFilter.CraftSkill.Id == null || searchFilter.CraftSkill.Id == 0 || cc.CraftSkillId == searchFilter.CraftSkill.Id)
                                             &&
                                             (
                                                 string.IsNullOrEmpty(searchFilter.Name) || c.Name.ToLower().Contains(searchFilter.Name.ToLower())
                                             )
                                         )
                                          select c
                            ).GroupBy(x => x.Id)
                            .Select(x => new CompanyDetailViewModel { Id = x.Key, Name = x.Max(m => m.Name) })
                            .AsQueryable();
                var paginatedCompanies = await companiesQueryable.Paginate(searchFilter);
                var filteredCompaniesIds = paginatedCompanies.Items.Select(x => x.Id);


                var companiesCraftSkills = await _db.CompanyCrafts.Include(x => x.CraftSkill).Where(x => filteredCompaniesIds.Contains(x.CompanyId)).ToListAsync();

                paginatedCompanies.Items.ForEach(x =>
                {
                    x.Crafts = _mapper.Map<List<BaseBriefVM>>(companiesCraftSkills.Where(u => u.CompanyId == x.Id).Select(x => x.CraftSkill).ToList());
                });
                var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<M>>();
                responseModel.ReturnModel = paginatedCompanies as PaginatedResultModel<M>;
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Company Service GetAll method threw an exception, Message: {ex.Message}");
                return Response.BadRequestResponse(_response);
            }
        }


        public async Task<bool> SetCompanyCraftSkills(List<long> craftSkillIds, long companyId)
        {
            try
            {
                var oldCompanyCraftSkills = await _db.CompanyCrafts.Where(x => x.CompanyId == companyId).ToListAsync();
                if (oldCompanyCraftSkills.Count() > 0)
                {
                    foreach (var oldComapnyCraft in oldCompanyCraftSkills)
                    {
                        oldComapnyCraft.IsDeleted = true;
                        _db.Entry(oldComapnyCraft).State = EntityState.Modified;
                    }
                    _db.SaveChanges();
                }
                if (craftSkillIds.Count() > 0)
                {
                    List<CompanyCraft> list = new List<CompanyCraft>();
                    foreach (var craftSkillId in craftSkillIds)
                    {
                        CompanyCraft companyCraft = new CompanyCraft();
                        companyCraft.CompanyId = companyId;
                        companyCraft.CraftSkillId = craftSkillId;
                        list.Add(companyCraft);
                    }
                    await _db.AddRangeAsync(list);
                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Company Service SetCompanyCraftSkills method threw an exception, Message: {ex.Message}");
                return false;
            }
        }



        public async Task<List<BaseBriefVM>> GetCompanyCraftSkills(long id)
        {
            try
            {
                var companyCrafts = await (from cc in _db.CompanyCrafts
                                           where cc.CompanyId == id
                                           join cs in _db.CraftSkills on cc.CraftSkillId equals cs.Id
                                           select new BaseBriefVM()
                                           {
                                               Id = cc.CraftSkillId,
                                               Name = cs.Name
                                           }).ToListAsync();
                return companyCrafts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Company Service GetCompanyCraftSkills method threw an exception, Message: {ex.Message}");
                return null;
            }
        }
    }
}
