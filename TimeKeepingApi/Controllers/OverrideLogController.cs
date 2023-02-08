
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination;
using Repositories.Services.CommonServices;
using Repositories.Services.OverrideLogServices.ORLogService;
using Repositories.Shared.UserInfoServices;
using TorranceApi.Controllers;
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

        public OverrideLogController(IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> oRLogService, IUserInfoService userInfoService, IMapper mapper) : base(oRLogService)
        {
            _oRLogService = oRLogService;
            _userInfoService = userInfoService;
            _mapper = mapper;
        }

        public override async Task<IActionResult> GetAll([FromQuery] ORLogAPISearchViewModel search)
        {
            var mappedSearchModel = _mapper.Map<ORLogSearchViewModel>(search);
            var result = await _oRLogService.GetAll<ORLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<ORLogDetailViewModel>>(result);
        }
        public override Task<IActionResult> Post([FromBody] ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            if (loggedInUserRole == "Employee")
            {
                ModelState.Remove("Requester.Id");
                ModelState.Remove("Requester.Name");
                // ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromBody] ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            if (loggedInUserRole == "Employee")
            {
               // ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Put(model);
        }
    }
}

