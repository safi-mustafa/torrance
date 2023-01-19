using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.DepartmentService;
using ViewModels.Common.Department;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : CrudBaseController<DepartmentCreateViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel, DepartmentDetailViewModel, DepartmentSearchViewModel>
    {
        public DepartmentController(IDepartmentService<DepartmentCreateViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel> departmentService) : base(departmentService)
        {
        }
    }
}

