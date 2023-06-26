
using System;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Helpers.ModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pagination;
using Repositories.Services.CommonServices;
using Repositories.Services.OverrideLogServices.ORLogService;
using Repositories.Shared.UserInfoServices;
using Repositories.Shared.VersionService;
using Select2.Model;
using Torrance.Api.Controllers;
using ViewModels.Common;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord;

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
        public override Task<IActionResult> Post([FromBody] ORLogModifyViewModel model)
        {
            ManagePostModelState(model);
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromBody] ORLogModifyViewModel model)
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
            ModelState.Remove("Company.Name");
            ModelState.Remove("ReasonForRequest");
            ModelState.Remove("DelayType");
            if (_versionService.GetVersionNumber().Equals("1.0"))
            {
                ModelState.Remove("EmployeeNames");
            }
            //if (model.Costs.Count < 1)
            //{
            //    ModelState.AddModelError("Costs.HeadCount","Please add atleast one cost row.");
            //}
        }
        private void ManagePutModelState(ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            if (loggedInUserRole == "Employee")
            {
                // ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            ModelState.Remove("ReasonForRequest");
            ModelState.Remove("DelayType");
            if (_versionService.GetVersionNumber().Equals("1.0"))
            {
                ModelState.Remove("EmployeeNames");
            }
            if (Version.Parse(_versionService.GetVersionNumber()) <= Version.Parse("1.0.1"))
            {
                ModelState.Remove("Costs");
                //ModelStateHelper.RemoveModelStateErrorsRecursive(ModelState, model, "Costs");
            }
            //if (model.Costs.Count < 1)
            //{
            //    ModelState.AddModelError("Costs.HeadCount", "Please add atleast one cost row.");
            //}
        }
    }
}

