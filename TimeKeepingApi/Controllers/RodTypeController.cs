﻿using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.RodType;
using Repositories.Services.WeldRodRecordServices.RodTypeService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RodTypeController : CrudBaseBriefController<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel, RodTypeDetailViewModel, RodTypeSearchViewModel>
    {
        public RodTypeController(IRodTypeService<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel> rodTypeService) : base(rodTypeService)
        {
        }
    }
}

