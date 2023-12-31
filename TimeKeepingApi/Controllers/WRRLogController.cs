﻿using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.WRRLog;
using Repositories.Services.AppSettingServices.WRRLogService;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;
using Repositories.Shared.UserInfoServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ViewModels.TimeOnTools.TOTLog;
using Repositories.Services.TimeOnToolServices.TOTLogService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WRRLogController : ApproveCrudBaseController<IWRRLogService<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel>, WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel, WRRLogDetailViewModel, WRRLogAPISearchViewModel>

    {
        private readonly IWRRLogService<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _wRRLogService;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public WRRLogController(IWRRLogService<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> wRRLogService, IMapper mapper, IUserInfoService userInfoService, ILogger<WRRLogController> logger) : base(wRRLogService, logger, "WRRLog")
        {
            _wRRLogService = wRRLogService;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }
        public override async Task<IActionResult> GetAll([FromQuery] WRRLogAPISearchViewModel search)
        {
            var mappedSearchModel = _mapper.Map<WRRLogSearchViewModel>(search);
            var result = await _wRRLogService.GetAll<WRRLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<WRRLogDetailViewModel>>(result);
        }

        [HttpPost]
        public override Task<IActionResult> Post([FromBody] WRRLogCreateViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == "Employee")
            {
                if (model.Company.Id < 1)
                {
                    var claims = User.Claims;
                    var companyIdClaim = claims.FirstOrDefault(c => c.Type == "CompanyId");
                    model.Company.Id = string.IsNullOrEmpty(companyIdClaim.Value) ? 0 : int.Parse(companyIdClaim.Value);
                }

                model.Employee.Id = parsedLoggedInId;
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            AddTWRModelStateErrors(ModelState, model.TWRModel);
            return base.Post(model);
        }

        [HttpPut]
        public override Task<IActionResult> Put([FromBody] WRRLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == "Employee")
            {
                if (model.Company.Id < 1)
                {
                    var claims = User.Claims;
                    var companyIdClaim = claims.FirstOrDefault(c => c.Type == "CompanyId");
                    model.Company.Id = string.IsNullOrEmpty(companyIdClaim.Value) ? 0 : int.Parse(companyIdClaim.Value);
                }
                model.Employee = new ViewModels.WeldingRodRecord.EmployeeBriefViewModel { Id = parsedLoggedInId, Name = "" };
            }
            ModelState.Remove("Contractor");
            ModelState.Remove("Contractor.Id");
            ModelState.Remove("Contractor.Name");
            AddTWRModelStateErrors(ModelState, model.TWRModel);
            return base.Put(model);
        }

        private void AddTWRModelStateErrors(ModelStateDictionary modelState, TWRViewModel model)
        {
            if (model.AlphabeticPart == null || string.IsNullOrEmpty(model.AlphabeticPart.id) || model.AlphabeticPart.id == "0")
                modelState.AddModelError("AlphabeticPart", "Required");
            if (model.NumericPart == null || string.IsNullOrEmpty(model.NumericPart.id) || model.NumericPart.id == "0")
                modelState.AddModelError("NumericPart", "Required");

        }
    }
}

