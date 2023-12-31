﻿using System;
using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TimeOnTools.PermitType;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermitTypeController : CrudBaseBriefController<PermitTypeDetailViewModel, PermitTypeSearchViewModel>
    {
        public PermitTypeController(IPermitTypeService<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel> permitTypeService, ILogger<PermitTypeController> logger) : base(permitTypeService, logger, "PermitType")
        {
        }
    }
}

