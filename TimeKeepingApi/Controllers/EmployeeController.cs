using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.Employee;
using Pagination;
using ViewModels.WeldingRodRecord.WRRLog;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.AppSettingServices.EmployeeService;
using Centangle.Common.ResponseHelpers.Models;
using System.Net;
using DocumentFormat.OpenXml.Office2010.Excel;
using ViewModels.Authentication.User;
using Centangle.Common.ResponseHelpers;
using DocumentFormat.OpenXml.Spreadsheet;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : CrudBaseBriefController<EmployeeDetailViewModel, EmployeeAPISearchViewModel>
    {
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService, IMapper mapper) : base(employeeService)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public override async Task<IActionResult> GetAll([FromQuery] EmployeeAPISearchViewModel search)
        {
            var mappedSearchModel = _mapper.Map<EmployeeSearchViewModel>(search);
            var result = await _employeeService.GetAll<EmployeeDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<EmployeeDetailViewModel>>(result);
        }
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Post([FromBody] EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = _mapper.Map<EmployeeModifyViewModel>(model);
                bool isUnique = await IsAccessCodeUnique(mappedModel);
                bool isEmailUnique = await IsEmailUnique(mappedModel);
                if (!isUnique)
                {
                    ModelState.AddModelError("AccessCode", "Access Code already in use.");
                }
                else if (!isEmailUnique)
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                }
                else
                {
                    mappedModel.Password = "TorrancePass";
                    mappedModel.ChangePassword = false;
                    var data = await _employeeService.Create(mappedModel);
                    if (data.Status == HttpStatusCode.OK)
                    {
                        var parsedResponse = data as RepositoryResponseWithModel<long>;
                        var id = parsedResponse?.ReturnModel ?? 0;
                        var response = new RepositoryResponseWithModel<UserCreateResponseViewModel>
                        {
                            ReturnModel = new UserCreateResponseViewModel
                            {
                                Id = id,
                                Message = "Account created successfully."// Please login using the pin used at the time of signup.
                            }
                        };
                        return ReturnProcessedResponse<UserCreateResponseViewModel>(response);
                    }
                    return ReturnProcessedResponse(data);
                }
            }
            return ReturnProcessedResponse(new RepositoryResponse { Status = HttpStatusCode.BadRequest });
        }

        private async Task<bool> IsAccessCodeUnique(EmployeeModifyViewModel model)
        {
            return await _employeeService.IsAccessCodeUnique(model.Id, model.AccessCode);

        }

        private async Task<bool> IsEmailUnique(EmployeeModifyViewModel model)
        {
            return await _employeeService.IsEmailUnique(model.Id, model.Email);

        }

    }
}

