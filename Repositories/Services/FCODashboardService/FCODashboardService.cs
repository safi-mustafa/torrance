using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using ViewModels;
using ViewModels.Dashboard;
using ViewModels.FCODashboard;

namespace Repositories.Services.DashboardService
{
    public class FCODashboardService : IFCODashboardService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<FCODashboardService> _logger;

        public FCODashboardService(ToranceContext db, ILogger<FCODashboardService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<FCOPieChartViewModel> GetFCOChartsData(FCOLogSearchViewModel search)
        {
            var model = new FCOPieChartViewModel();

            model.Status = await GetFilteredFCOLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.Status).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Status.ToString()),
                  Value = x.Sum(x => x.TotalCost)
              }).ToListAsync();

            model.Requestor = await GetFilteredFCOLogs(search).IgnoreQueryFilters()
              .Include(x => x.Employee)
              .GroupBy(x => x.EmployeeId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Employee.FullName),
                  Value = x.Sum(x => x.TotalCost)
              }).ToListAsync();

            model.Company = await GetFilteredFCOLogs(search).IgnoreQueryFilters()
              .Include(x => x.Company)
              .GroupBy(x => x.CompanyId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Company.Name),
                  Value = x.Sum(x => x.TotalCost)
              }).ToListAsync();

            return model;

        }

        private IQueryable<FCOLog> GetFilteredFCOLogs(FCOLogSearchViewModel search)
        {
            return _db.FCOLogs.Where(x =>
                    x.IsDeleted == false &&
                    (search.Requestor.Id == null || search.Requestor.Id == x.EmployeeId)
                    &&
                    (search.Unit.Id == null || search.Unit.Id == x.UnitId)
            );
        }
    }
}
