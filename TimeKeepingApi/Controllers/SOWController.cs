using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.SOW;
using Repositories.Services.TimeOnToolServices.SOWService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SOWController : CrudBaseBriefController<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel, SOWDetailViewModel, SOWSearchViewModel>
    {
        public SOWController(ISOWService<SOWModifyViewModel, SOWModifyViewModel, SOWDetailViewModel> sowService) : base(sowService)
        {
        }
    }
}

