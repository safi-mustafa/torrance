using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using ViewModels.WeldingRodRecord.Employee;
using Pagination;
using ViewModels.WeldingRodRecord.WRRLog;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

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

    }
}

