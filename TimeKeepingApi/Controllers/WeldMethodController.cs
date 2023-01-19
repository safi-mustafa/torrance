using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.WeldRodRecordServices.WeldMethodService;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeldMethodController : CrudBaseBriefController<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel, WeldMethodDetailViewModel, WeldMethodSearchViewModel>
    {
        public WeldMethodController(IWeldMethodService<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel> weldMethodService) : base(weldMethodService)
        {
        }
    }
}

