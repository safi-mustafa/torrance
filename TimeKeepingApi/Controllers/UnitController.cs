using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.UnitService;
using ViewModels.Common.Unit;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : CrudBaseController<UnitCreateViewModel, UnitModifyViewModel, UnitDetailViewModel, UnitDetailViewModel, UnitSearchViewModel>
    {
        public UnitController(IUnitService<UnitCreateViewModel, UnitModifyViewModel, UnitDetailViewModel> unitService) : base(unitService)
        {
        }
    }
}

