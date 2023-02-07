using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.TOTLog;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;
using Repositories.Shared.UserInfoServices;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.WeldingRodRecord;
using Select2.Model;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TOTLogController : ApproveCrudBaseController<ITOTLogService<TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel>, TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogAPISearchViewModel>
    {
        private readonly ITOTLogService<TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> _tOTLogService;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public TOTLogController(ITOTLogService<TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> tOTLogService, IMapper mapper, IUserInfoService userInfoService) : base(tOTLogService)
        {
            _tOTLogService = tOTLogService;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }

        public override async Task<IActionResult> GetAll([FromQuery] TOTLogAPISearchViewModel search)
        {
            var mappedSearchModel = _mapper.Map<TOTLogSearchViewModel>(search);
            var result = await _tOTLogService.GetAll<TOTLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<TOTLogDetailViewModel>>(result);
        }

        [HttpGet]
        [Route("~/api/[controller]/GetTWRNumericValues")]
        public async Task<IActionResult> GetTWRNumericValues([FromQuery] BaseSearchModel search)
        {
            var result = await _tOTLogService.GetTWRNumericValues<Select2ViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<Select2ViewModel>>(result);
        }
        [HttpGet]
        [Route("~/api/TOTLog/GetTWRAphabeticValues")]
        public async Task<IActionResult> GetTWRAphabeticValues([FromQuery] BaseSearchModel search)
        {
            var result = await _tOTLogService.GetTWRAphabeticValues<Select2ViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<Select2ViewModel>>(result);
        }

        public override Task<IActionResult> Post([FromBody] TOTLogCreateViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == "Employee")
            {
                model.Employee.Id = parsedLoggedInId;
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            return base.Post(model);
        }

        public override Task<IActionResult> Put([FromBody] TOTLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == "Employee")
            {
                model.Employee = new EmployeeBriefViewModel { Id = parsedLoggedInId, Name = "" };
            }
            return base.Put(model);
        }
    }
}

