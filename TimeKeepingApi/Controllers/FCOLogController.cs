using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;
using Repositories.Shared.UserInfoServices;
using Repositories.Services.AppSettingServices;
using ViewModels;
using Enums;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FCOLogController : CrudBaseController<FCOLogCreateViewModel, FCOLogModifyViewModel, FCOLogDetailViewModel, FCOLogDetailViewModel, FCOLogAPISearchViewModel>

    {
        private readonly IFCOLogService<FCOLogCreateViewModel, FCOLogModifyViewModel, FCOLogDetailViewModel> _fCOLogService;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public FCOLogController(IFCOLogService<FCOLogCreateViewModel, FCOLogModifyViewModel, FCOLogDetailViewModel> FCOLogService, IMapper mapper, IUserInfoService userInfoService) : base(FCOLogService)
        {
            _fCOLogService = FCOLogService;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }
        public override async Task<IActionResult> GetAll([FromQuery] FCOLogAPISearchViewModel search)
        {
            var mappedSearchModel = _mapper.Map<FCOLogSearchViewModel>(search);
            var result = await _fCOLogService.GetAll<FCOLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<FCOLogDetailViewModel>>(result);
        }

        public override Task<IActionResult> Post([FromForm] FCOLogCreateViewModel model)
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
            return base.Post(model);
        }

        [Authorize(Roles = ("Admin,SuperAdmin,Approver"))]
        [HttpPut("{id}/{status}/{approverId}/{comment}/{approverType}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Approve(long id, Status status, bool isUnauthenticatedApproval, long approverId, Guid? notificationId, string? comment, ApproverType approverType)
        {
            var result = await _fCOLogService.SetApproveStatus(id, status, isUnauthenticatedApproval, approverId, notificationId ?? new Guid(), comment ?? "", approverType);
            return ReturnProcessedResponse(result);
        }
        public override Task<IActionResult> Put([FromForm] FCOLogModifyViewModel model)
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
            return base.Put(model);
        }

        public async Task<IActionResult> GetFCOComments([FromQuery] long Id)
        {
            var comments = await _fCOLogService.GetFCOComments(Id);
            return ReturnProcessedResponse<List<FCOCommentsViewModel>>(comments);
        }

    }
}

