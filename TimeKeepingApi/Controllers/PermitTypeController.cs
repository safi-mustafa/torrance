using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.PermitType;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermitTypeController : CrudBaseBriefController<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel, PermitTypeDetailViewModel, PermitTypeSearchViewModel>
    {
        public PermitTypeController(IPermitTypeService<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

