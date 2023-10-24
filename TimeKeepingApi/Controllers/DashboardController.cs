using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pagination;
using Repositories.Services.DashboardService;
using Select2.Model;
using Torrance.Api.Controllers;
using ViewModels;
using ViewModels.Dashboard;
using ViewModels.TimeOnTools.TOTLog;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : TorranceController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboardService;

        public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService) : base(logger, "Dashboard")
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("~/api/Dashboard/GetTOTCharts")]
        public async Task<IActionResult> GetTOTCharts([FromQuery] TOTLogSearchViewModel search)
        {
            var result = await _dashboardService.GetAPITotChartsData(search);
            return ReturnProcessedResponse<TOTPieChartViewModel>(result);
        }
        [HttpGet]
        [Route("~/api/Dashboard/GetTotDelayTypeChartsData")]
        public async Task<IActionResult> GetTotDelayTypeChartsData([FromQuery] TOTLogSearchViewModel search)
        {
            var result = await _dashboardService.GetTotDelayTypeChartsData(search);
            return ReturnProcessedResponse<TOTWorkDelayTypeChartViewModel>(result);
        }
        [HttpGet]
        [Route("~/api/Dashboard/GetTotDelayTypeChartsCostsData")]
        public async Task<IActionResult> GetTotDelayTypeChartsCostsData([FromQuery] TOTLogSearchViewModel search)
        {
            var result = await _dashboardService.GetTotDelayTypeChartsCostsData(search);
            return ReturnProcessedResponse<TOTWorkDelayTypeChartViewModel>(result);
        }
    }
}
