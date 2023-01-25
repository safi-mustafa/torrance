using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.WRRLog;
using Repositories.Services.WeldRodRecordServices.WRRLogService;
using ViewModels.TomeOnTools.TOTLog;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WRRLogController : CrudBaseController<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel, WRRLogDetailViewModel, WRRLogAPISearchViewModel>
    {
        private readonly IWRRLogService<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _wRRLogService;
        private readonly IMapper _mapper;

        public WRRLogController(IWRRLogService<WRRLogCreateViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> wRRLogService, IMapper mapper) : base(wRRLogService)
        {
            _wRRLogService = wRRLogService;
            _mapper = mapper;
        }
        public override async Task<IActionResult> GetAll([FromQuery] WRRLogAPISearchViewModel search)
        {
            var mappedSearchModel = _mapper.Map<WRRLogSearchViewModel>(search);
            var result = await _wRRLogService.GetAll<WRRLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<WRRLogDetailViewModel>>(result);
        }

        
    }
}

