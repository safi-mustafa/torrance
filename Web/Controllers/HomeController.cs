using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pagination;
using Repositories.Services.DashboardService;
using System.Diagnostics;
using ViewModels.Dashboard;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardService _dashboardService;

        public HomeController(ILogger<HomeController> logger, IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult GetProgressChartData()
        {
            try
            {
                Object[] data = {
                    new { x = "Jan", value1 = 0.5,value2 = 1, value3 = 1.2},
                    new { x = "Feb", value1 = 1,value2 = 1,value3 = 2.2},
                    new { x = "March", value1 = 0.5,value2 = 1, value3 = 3.2},
                    new { x = "April", value1 = 2,value2 = 1, value3 = 4.2}
                };
                return Json(data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetPieChartdata()
        {
            try
            {
                Object[] data = {
                    new { x = "Shift1", value1 = 2},
                      new { x = "Shift2", value1 = 4},

                };
                return Json(data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> GetTotChartsData()
        {
            var data = await _dashboardService.GetTotChartsData();
            return Json(data);
        }

        public async Task<ActionResult> GetWrrChartsData()
        {
            var data = await _dashboardService.GetWrrChartsData();
            return Json(data);
        }

    }
}