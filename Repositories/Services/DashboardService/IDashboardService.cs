using ViewModels.Dashboard;

namespace Repositories.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<TOTPieChartViewModel> GetTotChartsData();
        Task<WrrPieChartViewModel> GetWrrChartsData();
    }
}
