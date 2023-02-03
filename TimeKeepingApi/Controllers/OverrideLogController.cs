
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices;
using Repositories.Services.OverrideLogServices.ORLogService;
using Repositories.Shared.UserInfoServices;
using TorranceApi.Controllers;
using ViewModels.Common;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.WeldingRodRecord;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OverrideLogController : CrudBaseController<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel, ORLogDetailViewModel, ORLogSearchViewModel>
    {
        private readonly IUserInfoService _userInfoService;

        public OverrideLogController(IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> ORLogService, IUserInfoService userInfoService) : base(ORLogService)
        {
            _userInfoService = userInfoService;
        }

        public override Task<IActionResult> Post([FromBody] ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == "Employee")
            {
                model.Requester.Id = parsedLoggedInId;
                ModelState.Remove("Requester.Id");
                ModelState.Remove("Requester.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromBody] ORLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == "Employee")
            {
                model.Requester = new EmployeeBriefViewModel { Id = parsedLoggedInId, Name = "" };
            }
            ModelState.Remove("Company.Name");
            return base.Put(model);
        }
    }
}

