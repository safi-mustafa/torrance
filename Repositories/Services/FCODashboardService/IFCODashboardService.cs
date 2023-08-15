using ViewModels;
using ViewModels.FCODashboard;

namespace Repositories.Services.DashboardService
{
    public interface IFCODashboardService
    {
        Task<FCOPieChartViewModel> GetFCOChartsData(FCOLogSearchViewModel search);
    }
}
