using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Employee;

namespace Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class EmployeeController : CrudBaseController<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel, EmployeeDetailViewModel, EmployeeSearchViewModel>
    {
        private readonly IEmployeeService _EmployeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService EmployeeService, ILogger<EmployeeController> logger, IMapper mapper) : base(EmployeeService, logger, mapper, "Employee", "Employees")
        {
            _EmployeeService = EmployeeService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "First Name",data = "FirstName"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
