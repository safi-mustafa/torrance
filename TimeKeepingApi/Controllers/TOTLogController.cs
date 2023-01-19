using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.TOTLog;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using AutoMapper;
using Pagination;
using ViewModels.WeldingRodRecord.WRRLog;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TOTLogController : CrudBaseController<TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogAPISearchViewModel>
    {
        private readonly ITOTLogService<TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> _tOTLogService;
        private readonly IMapper _mapper;

        public TOTLogController(ITOTLogService<TOTLogCreateViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> tOTLogService, IMapper mapper) : base(tOTLogService)
        {
            _tOTLogService = tOTLogService;
            _mapper = mapper;
        }

        public override async Task<IActionResult> GetAll([FromQuery] TOTLogAPISearchViewModel search)
        {
            var mappedSearchModel = _mapper.Map<TOTLogSearchViewModel>(search);
            var result = await _tOTLogService.GetAll<TOTLogDetailViewModel>(mappedSearchModel);
            return ReturnProcessedResponse<PaginatedResultModel<TOTLogDetailViewModel>>(result);
        }

    }
}

