using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.WeldRodRecordServices.WeldMethodService;
using ViewModels.WeldingRodRecord.WeldMethod;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeldMethodController : CrudBaseBriefController<WeldMethodDetailViewModel, WeldMethodSearchViewModel>
    {
        public WeldMethodController(IWeldMethodService<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel> weldMethodService) : base(weldMethodService)
        {
        }
    }
}

