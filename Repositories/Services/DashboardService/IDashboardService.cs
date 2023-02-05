using ViewModels.Dashboard;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<TOTPieChartViewModel> GetTotChartsData(TOTLogSearchViewModel search);

        Task<StatusChartViewModel> GetTotStatusChartData(TOTLogSearchViewModel search);

        Task<StatusChartViewModel> GetOverrideStatusChartData(TOTLogSearchViewModel search);
        Task<WrrPieChartViewModel> GetWrrChartsData(WRRLogSearchViewModel search);
        Task<DashboardViewModel> GetDashboardData();
    }
}
