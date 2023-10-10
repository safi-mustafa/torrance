using Centangle.Common.ResponseHelpers.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
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
        
        public DashboardViewModel DashboardStats { get; set; }
        public TOTPieChartViewModel DashboardTOTModel { get; set; }
        public OverridePieChartViewModel DashboardORModel { get; set; }
        public APIBarChartsVM DashboardBarChartModel { get; set; }
        public ApproverDashboardModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task OnGetAsync(TOTLogSearchViewModel search)
        {
            var stats = await _dashboardService.GetDashboardData();
            if (stats!= null)
            {
                DashboardStats = stats;
            }
            var totResult = await _dashboardService.GetAPITotChartsData(search);
            if (totResult.Status == System.Net.HttpStatusCode.OK)
            {
                var responseModel = (totResult as RepositoryResponseWithModel<TOTPieChartViewModel>);
                DashboardTOTModel = responseModel.ReturnModel;
            }

            var orResult = await _dashboardService.GetAPIOverrideChartsData(search);
            if (orResult.Status == System.Net.HttpStatusCode.OK)
            {
                var responseModel = (orResult as RepositoryResponseWithModel<OverridePieChartViewModel>);
                DashboardORModel = responseModel.ReturnModel;
            }

            var barChartsData = await _dashboardService.GetAPIBarChartsData();
            if (barChartsData.Status == System.Net.HttpStatusCode.OK)
            {
                var responseModel = (barChartsData as RepositoryResponseWithModel<APIBarChartsVM>);
                DashboardBarChartModel = responseModel.ReturnModel;
            }
        }

       
    }
}
