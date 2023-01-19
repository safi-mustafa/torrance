using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.PermitType;
using Repositories.Services.TimeOnToolServices.PermitTypeService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermitTypeController : CrudBaseBriefController<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel, PermitTypeDetailViewModel, PermitTypeSearchViewModel>
    {
        public PermitTypeController(IPermitTypeService<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

