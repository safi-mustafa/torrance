﻿using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.OverrideTypeService;
using ViewModels.OverrideLogs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OverrideTypeController : CrudBaseBriefController<OverrideTypeModifyViewModel, OverrideTypeModifyViewModel, OverrideTypeDetailViewModel, OverrideTypeDetailViewModel, OverrideTypeSearchViewModel>
    {
        public OverrideTypeController(IOverrideTypeService<OverrideTypeModifyViewModel, OverrideTypeModifyViewModel, OverrideTypeDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}
