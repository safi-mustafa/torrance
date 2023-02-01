using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

    }
}