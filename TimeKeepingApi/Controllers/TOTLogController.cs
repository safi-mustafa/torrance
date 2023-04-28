using BainBridgeApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.TOTLog;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;
using Repositories.Shared.UserInfoServices;
using ViewModels.WeldingRodRecord;
using Select2.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            search.OrderByColumn = "CreatedOn";
            search.OrderDir = PaginationOrderCatalog.Desc;
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
            ModelState.Remove("ReasonForRequest");
            model.Validate(ModelState);
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
            model.Validate(ModelState);
            ModelState.Remove("Contractor");
            ModelState.Remove("DelayType");
            ModelState.Remove("ShiftDelay");
            ModelState.Remove("ReworkDelay");
            ModelState.Remove("PermittingIssue");
            ModelState.Remove("Department");
            ModelState.Remove("ReasonForRequest");
            return base.Put(model);
        }

        [HttpGet]
        [Route("~/api/[controller]/GetDelayReasons")]
        public async Task<IActionResult> GetDelayReasons([FromQuery] BaseSearchModel search)
        {
            var result = await _tOTLogService.GetDelayReason<Select2ViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<Select2ViewModel>>(result);
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

