using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.OverrideLogs;
using Pagination;
using Repositories.Services.OverrideLogServices.ORLogService;
using Repositories.Shared.UserInfoServices;
using Repositories.Shared.VersionService;
using Select2.Model;
using Torrance.Api.Controllers;
using ViewModels.OverrideLogs;
using ViewModels.OverrideLogs.ORLog;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OverrideLogController : ApproveCrudBaseController<IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel>, ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel, ORLogDetailViewModel, ORLogAPISearchViewModel>
    {
        private readonly IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> _oRLogService;
        private readonly IUserInfoService _userInfoService;
        private readonly IMapper _mapper;
        private readonly IVersionService _versionService;

        public OverrideLogController(
            IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> oRLogService
            , IUserInfoService userInfoService
            , IMapper mapper
            , IVersionService versionService
            ) : base(oRLogService)
        {
            _oRLogService = oRLogService;
            _userInfoService = userInfoService;
            _mapper = mapper;
            _versionService = versionService;
        }

        public override async Task<IActionResult> GetAll([FromQuery] ORLogAPISearchViewModel search)
        {
            search.OrderByColumn = "CreatedOn";
            search.OrderDir = PaginationOrderCatalog.Desc;
            var mappedSearchModel = _mapper.Map<ORLogSearchViewModel>(search);
            var result = await _oRLogService.GetAll<ORLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<ORLogDetailViewModel>>(result);
        }
        public override Task<IActionResult> Post([FromForm] ORLogModifyViewModel model)
        {
            ManagePostModelState(model);
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromForm] ORLogModifyViewModel model)
        {
            ManagePutModelState(model);
            return base.Put(model);
        }

        [HttpGet]
        [Route("~/api/[controller]/GetOverrideTypes")]
        public async Task<IActionResult> GetOverrideTypes([FromQuery] BaseSearchModel search)
        {
            var result = await _oRLogService.GetOverrideTypes<Select2ViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<Select2ViewModel>>(result);
        }

        private void ManagePostModelState(ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            if (loggedInUserRole == "Employee")
            {
                ModelState.Remove("Requester.Id");
                ModelState.Remove("Requester.Name");
                // ModelState.Remove("Approver.Name");
            }
            ManageCommonModelState(model);
        }
        private void ManagePutModelState(ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            if (loggedInUserRole == "Employee")
            {
                // ModelState.Remove("Approver.Name");
            }
            ManageCommonModelState(model);
        }

        private void ManageCommonModelState(ORLogModifyViewModel model)
        {
            var version = Version.Parse(_versionService.GetVersionNumber());
            ModelState.Remove("Company.Name");
            ModelState.Remove("ReasonForRequest");
            ModelState.Remove("DelayType");
            if (version <= Version.Parse("0.0.0"))
            {
                ModelState.Remove("EmployeeNames");
            }
            if (version < Version.Parse("1.0.1"))
            {
                ModelState.Remove("Costs");
                var keysToRemove = ModelState.Keys.Where(k => k.StartsWith("Costs[")).ToList();
                foreach (var key in keysToRemove)
                {
                    ModelState.Remove(key);
                }
            }
            if (version < Version.Parse("1.0.2"))
            {
                for (var i = 0; i < model.Costs.Count; i++)
                {

                    if (model.Costs[i].STHours < 1 && model.Costs[i].OTHours < 1 && model.Costs[i].DTHours < 1)
                    {
                        ModelState.AddModelError($"Costs[{i}]", "Cost must have at least one ST, OT or DT hour.");
                    }
                }
            }
        }

        private List<ORLogCostViewModel> GroupCosts(List<ORLogCostViewModel> costs)
        {
            var mergedCosts = new List<ORLogCostViewModel>();
            foreach (var c in costs.GroupBy(x => x.CraftSkill.Id))
            {
                var mergedCost = new ORLogCostViewModel();
                foreach (var i in c)
                {
                    //mapping hours on the basis of Override Type
                    switch (i.OverrideType)
                    {
                        case OverrideTypeCatalog.ST: mergedCost.STHours = i.OverrideHours; break;
                        case OverrideTypeCatalog.OT: mergedCost.OTHours = i.OverrideHours; break;
                        case OverrideTypeCatalog.DT: mergedCost.DTHours = i.OverrideHours; break;
                    }
                }
                //mapping common fields
                mergedCost.CraftSkill = c.Max(x => x.CraftSkill) ?? new();
                mergedCost.OverrideLogId = c.Max(x => x.OverrideLogId);
                mergedCost.HeadCount = c.Max(x => x.HeadCount);
                mergedCosts.Add(mergedCost);
            }
            return mergedCosts;
        }

        private List<ORLogCostViewModel> UnGroupCosts(List<ORLogCostViewModel> costs)
        {
            var mergedCosts = new List<ORLogCostViewModel>();
            foreach (var c in costs)
            {
                //un grouping on row in to multiple based on the Hours added, i.e. for STHours an object with OverrideType ST and STHours will be mapped to OverrideHours.
                if (c.STHours > 0)
                {
                    CreateNewCostObject(mergedCosts, c, OverrideTypeCatalog.ST);
                }
                if (c.OTHours > 0)
                {
                    CreateNewCostObject(mergedCosts, c, OverrideTypeCatalog.OT);
                }
                if (c.DTHours > 0)
                {
                    CreateNewCostObject(mergedCosts, c, OverrideTypeCatalog.DT);
                }
            }
            return mergedCosts;
        }

        private static void CreateNewCostObject(List<ORLogCostViewModel> mergedCosts, ORLogCostViewModel c, OverrideTypeCatalog oRType)
        {
            var mergedCost = c.CreateShallowCopy();

            //mapping fields
            mergedCost.OverrideType = oRType;
            mergedCost.CraftSkill = c.CraftSkill;
            mergedCost.OverrideLogId = c.OverrideLogId;
            mergedCost.HeadCount = c.HeadCount;
            mergedCosts.Add(mergedCost);
        }
    }
}

