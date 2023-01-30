using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.TOTLog;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;
using Repositories.Shared.UserInfoServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TOTLogController : CrudBaseController<TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogAPISearchViewModel>
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
                model.Employee = new ViewModels.WeldingRodRecord.Employee.EmployeeBriefViewModel { Id = parsedLoggedInId, Name = "" };
            }
            return base.Put(model);
        }
    }
}

