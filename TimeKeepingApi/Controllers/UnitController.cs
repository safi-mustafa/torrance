using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.UnitService;
using ViewModels.Common.Unit;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : CrudBaseBriefController<UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel, UnitDetailViewModel, UnitSearchViewModel>
    {
        public UnitController(IUnitService<UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel> unitService) : base(unitService)
        {
        }
    }
}

