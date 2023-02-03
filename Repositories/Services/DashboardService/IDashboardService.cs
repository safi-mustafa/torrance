using ViewModels.Dashboard;
using ViewModels.TomeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<TOTPieChartViewModel> GetTotChartsData(TOTLogSearchViewModel search);
        Task<WrrPieChartViewModel> GetWrrChartsData(WRRLogSearchViewModel search);
        Task<DashboardViewModel> GetDashboardData();
    }
}
