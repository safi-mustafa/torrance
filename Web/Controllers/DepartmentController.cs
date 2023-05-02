using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.DepartmentService;
using ViewModels.Common.Department;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class DepartmentController : CrudBaseController<DepartmentModifyViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel, DepartmentDetailViewModel, DepartmentSearchViewModel>
    {
        private readonly IDepartmentService<DepartmentModifyViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel> _DepartmentService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentService<DepartmentModifyViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel> DepartmentService, ILogger<DepartmentController> logger, IMapper mapper) : base(DepartmentService, logger, mapper, "Department", "Departments")
        {
            _DepartmentService = DepartmentService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "Units",data = "FormattedUnits", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
