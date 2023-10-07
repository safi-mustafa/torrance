using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Services.DashboardService;
using ViewModels.Dashboard;
using ViewModels.TimeOnTools.TOTLog;

namespace API.Pages
{
    public class ApproverDashboardModel : PageModel
    {
        private readonly IDashboardService _dashboardService;
        public TOTWorkDelayTypeChartViewModel DashboarModel { get; set; }
        public ApproverDashboardModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task OnGetAsync(TOTLogSearchViewModel search)
        {
            var result = await _dashboardService.GetTotDelayTypeChartsData(search);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                var responseModel = (result as RepositoryResponseWithModel<TOTWorkDelayTypeChartViewModel>);
                DashboarModel = responseModel.ReturnModel;
            }
        }
    }
}
