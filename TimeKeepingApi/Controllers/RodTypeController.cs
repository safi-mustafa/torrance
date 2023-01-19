using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.RodType;
using Repositories.Services.WeldRodRecordServices.RodTypeService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RodTypeController : CrudBaseBriefController<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel, RodTypeDetailViewModel, RodTypeSearchViewModel>
    {
        public RodTypeController(IRodTypeService<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel> rodTypeService) : base(rodTypeService)
        {
        }
    }
}

