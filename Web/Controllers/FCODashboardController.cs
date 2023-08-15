using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.DashboardService;
using ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class FCODashboardController : Controller
    {
        private readonly ILogger<FCODashboardController> _logger;
        private readonly IFCODashboardService _dashboardService;

        public FCODashboardController(ILogger<FCODashboardController> logger, IFCODashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }
        [Authorize(Roles = "SuperAdmin,Administrator")]
      
        public async Task<ActionResult> Index(FCOLogSearchViewModel search)
        {
            var data = await _dashboardService.GetFCOChartsData(search);
            return Json(data);
        }
    }
}