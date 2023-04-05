using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.Employee;
using Pagination;
using ViewModels.WeldingRodRecord.WRRLog;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.AppSettingServices.EmployeeService;
using Centangle.Common.ResponseHelpers.Models;
using System.Net;

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
                if (!isUnique)
                {
                    ModelState.AddModelError("AccessCode", "Access Code already in use or illegal Access Code.");
                }
                else
                {
                    mappedModel.Password = "TorrancePass";
                    mappedModel.ChangePassword = false;
                    var data = await _employeeService.Create(mappedModel);
                    return ReturnProcessedResponse(data);
                }
            }
            return ReturnProcessedResponse(new RepositoryResponse { Status = HttpStatusCode.BadRequest });
        }

        private async Task<bool> IsAccessCodeUnique(EmployeeModifyViewModel model)
        {
            return await _employeeService.IsAccessCodeUnique(model.Id, model.AccessCode);

        }

    }
}

