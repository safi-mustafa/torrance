using Centangle.Common.ResponseHelpers.Models;
using ViewModels.Dashboard;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardData();
        Task<StatusChartViewModel> GetTotStatusChartData(TOTLogSearchViewModel search);
        Task<StatusChartViewModel> GetOverrideStatusChartData(TOTLogSearchViewModel search);
        Task<StatusChartViewModel> GetWrrStatusChartData(WRRLogSearchViewModel search);
        Task<TOTPieChartViewModel> GetTotChartsData(TOTLogSearchViewModel search);
        Task<OverridePieChartViewModel> GetOverrideChartsData(TOTLogSearchViewModel search);
        Task<IRepositoryResponse> GetAPITotChartsData(TOTLogSearchViewModel search);
        Task<IRepositoryResponse> GetAPIOverrideChartsData(TOTLogSearchViewModel search);
        Task<IRepositoryResponse> GetAPIBarChartsData();
    }
}
