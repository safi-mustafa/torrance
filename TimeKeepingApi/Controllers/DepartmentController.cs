using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.DepartmentService;
using ViewModels.Common.Department;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : CrudBaseBriefController<DepartmentModifyViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel, DepartmentDetailViewModel, DepartmentSearchViewModel>
    {
        public DepartmentController(IDepartmentService<DepartmentModifyViewModel, DepartmentModifyViewModel, DepartmentDetailViewModel> departmentService) : base(departmentService)
        {
        }
    }
}

