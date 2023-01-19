using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.SOW;
using Repositories.Services.TimeOnToolServices.SOWService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SOWController : CrudBaseBriefController<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel, SOWDetailViewModel, SOWSearchViewModel>
    {
        public SOWController(ISOWService<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel> sowService) : base(sowService)
        {
        }
    }
}

