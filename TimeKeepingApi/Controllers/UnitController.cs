using System;
using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.UnitService;
using ViewModels.Common.Unit;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitController : CrudBaseBriefController<UnitDetailViewModel, UnitSearchViewModel>
    {
        public UnitController(IUnitService<UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel> unitService) : base(unitService)
        {
        }
    }
}

