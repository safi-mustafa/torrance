﻿using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.CraftRateService;
using ViewModels.OverrideLogs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CraftRateController : CrudBaseBriefController<CraftRateModifyViewModel, CraftRateModifyViewModel, CraftRateDetailViewModel, CraftRateDetailViewModel, CraftRateSearchViewModel>
    {
        public CraftRateController(ICraftRateService<CraftRateModifyViewModel, CraftRateModifyViewModel, CraftRateDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

