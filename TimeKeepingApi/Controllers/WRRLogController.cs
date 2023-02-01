using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.WRRLog;
using Repositories.Services.WeldRodRecordServices.WRRLogService;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;
using Repositories.Shared.UserInfoServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WRRLogController : CrudBaseController<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel, WRRLogDetailViewModel, WRRLogAPISearchViewModel>
    {
        private readonly IWRRLogService<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _wRRLogService;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public WRRLogController(IWRRLogService<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> wRRLogService, IMapper mapper, IUserInfoService userInfoService) : base(wRRLogService)
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

        public override Task<IActionResult> Post([FromBody] WRRLogCreateViewModel model)
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

        public override Task<IActionResult> Put([FromBody] WRRLogModifyViewModel model)
        {
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);
            if (loggedInUserRole == "Employee")
            {
                model.Employee = new ViewModels.WeldingRodRecord.EmployeeBriefViewModel { Id = parsedLoggedInId, Name = "" };
            }
            return base.Put(model);
        }
    }
}

