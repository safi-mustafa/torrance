using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.OverrideLogs;
using Newtonsoft.Json;
using Pagination;
using Repositories.Services.OverrideLogServices.ORLogService;
using Repositories.Shared.UserInfoServices;
using Repositories.Shared.VersionService;
using Select2.Model;
using Torrance.Api.Controllers;
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
        private readonly ILogger<OverrideLogController> _logger;
        private readonly Version _version;

        public OverrideLogController(
            IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> oRLogService
            , IUserInfoService userInfoService
            , IMapper mapper
            , IVersionService versionService
            , ILogger<OverrideLogController> logger
            ) : base(oRLogService, logger, "OverrideLog")
        {
            _oRLogService = oRLogService;
            _userInfoService = userInfoService;
            _mapper = mapper;
            _versionService = versionService;
            _logger = logger;
            _version = Version.Parse(_versionService.GetVersionNumber());
        }

        public override async Task<IActionResult> GetAll([FromQuery] ORLogAPISearchViewModel search)
        {
            search.OrderByColumn = "CreatedOn";
            search.OrderDir = PaginationOrderCatalog.Desc;
            var mappedSearchModel = _mapper.Map<ORLogSearchViewModel>(search);
            var result = await _oRLogService.GetAll<ORLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<ORLogDetailViewModel>>(result);
        }

        public async override Task<IActionResult> Get(long id)
        {

            var result = await _oRLogService.GetById(id);
            var responseModel = result as RepositoryResponseWithModel<ORLogDetailViewModel>;
            var model = responseModel?.ReturnModel;

            if (_version < Version.Parse("1.0.2"))
            {
                if (model != null)
                {
                    model.Costs = UnGroupCosts(model.Costs);
                }
            }
            return ReturnProcessedResponse<ORLogDetailViewModel>(responseModel);
        }

        [HttpPost]
        public override Task<IActionResult> Post([FromForm] ORLogModifyViewModel model)
        {
            ManagePostModelStateAndVersionChanges(model);
            _logger.LogInformation(JsonConvert.SerializeObject(model));
            return base.Post(model);
        }

        [HttpPut]
        public override Task<IActionResult> Put([FromForm] ORLogModifyViewModel model)
        {
            ManagePutModelStateAndVersionChanges(model);
            return base.Put(model);
        }

        [HttpGet]
        [Route("~/api/[controller]/GetOverrideTypes")]
        public async Task<IActionResult> GetOverrideTypes([FromQuery] BaseSearchModel search)
        {
            var result = await _oRLogService.GetOverrideTypes<Select2ViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<Select2ViewModel>>(result);
        }

        private void ManagePostModelStateAndVersionChanges(ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            if (loggedInUserRole == "Employee")
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
                // ModelState.Remove("Approver.Name");
            }
            ManageCommonModelStateAndVersionChanges(model);
        }

        private void ManagePutModelStateAndVersionChanges(ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            if (loggedInUserRole == "Employee")
            {
                // ModelState.Remove("Approver.Name");
            }
            ManageCommonModelStateAndVersionChanges(model);
        }

        private void ManageCommonModelStateAndVersionChanges(ORLogModifyViewModel model)
        {
            ModelState.Remove("Company.Name");
            ModelState.Remove("ReasonForRequest");
            ModelState.Remove("DelayType");
            ModelState.Remove("ActiveStatus");
            ModelState.Remove("IsArchived");
            if (_version <= Version.Parse("0.0.0"))
            {
                ModelState.Remove("EmployeeNames");
            }
            if (_version < Version.Parse("1.0.1"))
            {
                ModelState.Remove("Costs");
                var keysToRemove = ModelState.Keys.Where(k => k.StartsWith("Costs[")).ToList();
                foreach (var key in keysToRemove)
                {
                    ModelState.Remove(key);
                }
            }
            if (_version < Version.Parse("1.0.2"))
            {
                model.Costs = GroupCosts(model.Costs);
            }
            if (_version >= Version.Parse("1.0.2"))
            {
                for (var i = 0; i < model.Costs.Count; i++)
                {
                    if (model.Costs[i].STHours < 0 || model.Costs[i].OTHours < 0 || model.Costs[i].DTHours < 0)
                    {
                        ModelState.AddModelError($"Costs[{i}].general", "Cost must have hours greater than 0 for ST, OT and DT hour.");
                    }
                    if (model.Costs[i].STHours <= 0 && model.Costs[i].OTHours <= 0 && model.Costs[i].DTHours <= 0)
                    {
                        ModelState.AddModelError($"Costs[{i}].general", "Cost must have at least one ST, OT or DT hour.");
                    }
                }
                var keysToRemove = ModelState.Keys.Where(k => k.StartsWith("Costs[") && (k.EndsWith("].OverrideType") || k.EndsWith("].OverrideHours"))).ToList();
                foreach (var key in keysToRemove)
                {
                    ModelState.Remove(key);
                }
            }
        }

        private List<ORLogCostViewModel> GroupCosts(List<ORLogCostViewModel> costs)
        {
            //var mergedCosts = new List<ORLogCostViewModel>();
            //foreach (var c in costs.GroupBy(x => x.CraftSkill.Id))
            //{
            //    var mergedCost = new ORLogCostViewModel();
            //    foreach (var i in c)
            //    {
            //        //mapping hours on the basis of Override Type
            //        switch (i.OverrideType)
            //        {
            //            case OverrideTypeCatalog.ST: mergedCost.STHours = i.OverrideHours; break;
            //            case OverrideTypeCatalog.OT: mergedCost.OTHours = i.OverrideHours; break;
            //            case OverrideTypeCatalog.DT: mergedCost.DTHours = i.OverrideHours; break;
            //        }
            //    }
            //    //mapping common fields
            //    mergedCost.CraftSkill = c.Max(x => x.CraftSkill) ?? new();
            //    mergedCost.OverrideLogId = c.Max(x => x.OverrideLogId);
            //    mergedCost.HeadCount = c.Max(x => x.HeadCount);
            //    mergedCosts.Add(mergedCost);
            //}
            //return mergedCosts;
            var mergedCosts = new List<ORLogCostViewModel>();
            foreach (var c in costs)
            {
                //mapping the old cost format on to new one, as per discussion, 
                var mergedCost = new ORLogCostViewModel();
                mergedCost.STHours = 0;
                mergedCost.OTHours = 0;
                mergedCost.DTHours = 0;
                //mapping hours on the basis of Override Type
                switch (c.OverrideType)
                {
                    case OverrideTypeCatalog.ST: mergedCost.STHours = c.OverrideHours; break;
                    case OverrideTypeCatalog.OT: mergedCost.OTHours = c.OverrideHours; break;
                    case OverrideTypeCatalog.DT: mergedCost.DTHours = c.OverrideHours; break;
                }
                //mapping common fields
                mergedCost.CraftSkill = c.CraftSkill;
                mergedCost.OverrideLogId = c.OverrideLogId;
                mergedCost.HeadCount = c.HeadCount;
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
                    CreateNewCostObject(mergedCosts, c, OverrideTypeCatalog.ST, c.STHours ?? 0);
                }
                if (c.OTHours > 0)
                {
                    CreateNewCostObject(mergedCosts, c, OverrideTypeCatalog.OT, c.OTHours ?? 0);
                }
                if (c.DTHours > 0)
                {
                    CreateNewCostObject(mergedCosts, c, OverrideTypeCatalog.DT, c.DTHours ?? 0);
                }
            }
            return mergedCosts;
        }

        private static void CreateNewCostObject(List<ORLogCostViewModel> mergedCosts, ORLogCostViewModel c, OverrideTypeCatalog oRType, double hours)
        {
            var mergedCost = c.CreateShallowCopy();

            //mapping fields
            mergedCost.OverrideType = oRType;
            mergedCost.CraftSkill = c.CraftSkill;
            mergedCost.OverrideLogId = c.OverrideLogId;
            mergedCost.HeadCount = c.HeadCount;
            mergedCost.OverrideHours = hours;
            mergedCosts.Add(mergedCost);
        }
    }
}

