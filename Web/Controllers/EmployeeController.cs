using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Employee;

namespace Web.Controllers
{
     [Authorize(Roles = "SuperAdmin")]
    public class EmployeeController : CrudBaseController<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel, EmployeeDetailViewModel, EmployeeSearchViewModel>
    {
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService, ILogger<EmployeeController> logger, IMapper mapper) : base(employeeService, logger, mapper, "Employee", "Employees")
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "First Name",data = "FirstName"},
                new DataTableViewModel{title = "Last Name",data = "LastName"},
                new DataTableViewModel{title = "Employee Id",data = "EmployeeId"},
                new DataTableViewModel{title = "Status",data = "FormattedStatus"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
        public override async Task<ActionResult> Create(EmployeeModifyViewModel model)
        {
            bool isUnique = await _employeeService.IsEmployeeIdUnique(model.Id, model.EmployeeId);
            if (!isUnique)
            {
                ModelState.AddModelError("EmployeeId", "Employee Id already in use.");
            }
            return await base.Create(model);
        }
        public override async Task<ActionResult> Update(EmployeeModifyViewModel model)
        {
            bool isUnique = await _employeeService.IsEmployeeIdUnique(model.Id, model.EmployeeId);
            if (!isUnique)
            {
                ModelState.AddModelError("EmployeeId", "Employee Id already in use.");
            }
            return await base.Update(model);
        }
        public async Task<IActionResult> ValidateEmployeeId(int id, string employeeId)
        {
            return Json(await _employeeService.IsEmployeeIdUnique(id, employeeId));
        }
        public async Task<IActionResult> ValidateEmployeeEmail(int id, string email)
        {
            return Json(await _employeeService.IsEmployeeEmailUnique(id, email));
        }
    }
}
