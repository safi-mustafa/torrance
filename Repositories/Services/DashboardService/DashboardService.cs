using DataLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Dashboard;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(ToranceContext db, ILogger<DashboardService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<TOTPieChartViewModel> GetTotChartsData(TOTLogSearchViewModel search)
        {
            var model = new TOTPieChartViewModel();
            var totCount = await GetFilteredTOTLogs(search).CountAsync();
            model.ShiftDelays = await GetFilteredTOTLogs(search)
              .Include(x => x.ShiftDelay)
              .GroupBy(x => x.ShiftDelayId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.ShiftDelay.Name),
                  Value = x.Count() * 100 / totCount
              }).ToListAsync();

            model.ReworkDelays = await GetFilteredTOTLogs(search)
                .Include(x => x.ReworkDelay)
                .GroupBy(x => x.ReworkDelayId).Select(x => new LogPieChartViewModel
                {
                    Category = x.Max(y => y.ReworkDelay.Name),
                    Value = x.Count() * 100 / totCount
                }).ToListAsync();

            return model;

        }

        public async Task<StatusChartViewModel> GetTotStatusChartData(TOTLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredTOTLogs(search)
              .GroupBy(x => x.Status).Select(x => new BarChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).ToListAsync();

            return model;

        }

        public async Task<StatusChartViewModel> GetOverrideStatusChartData(TOTLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredORLogs(search)
              .GroupBy(x => x.Status).Select(x => new BarChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).ToListAsync();

            return model;

        }

        public async Task<WrrPieChartViewModel> GetWrrChartsData(WRRLogSearchViewModel search)
        {
            var model = new WrrPieChartViewModel();
            var totCount = await GetFilteredWrrLogs(search).CountAsync();
            model.WeldMethods = await GetFilteredWrrLogs(search)
              .Include(x => x.WeldMethod)
              .GroupBy(x => x.WeldMethodId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.WeldMethod.Name),
                  Value = x.Count() * 100 / totCount
              }).ToListAsync();

            model.RodTypes = await GetFilteredWrrLogs(search)
                .Include(x => x.RodType)
                .GroupBy(x => x.RodTypeId).Select(x => new LogPieChartViewModel
                {
                    Category = x.Max(y => y.RodType.Name),
                    Value = x.Count() * 100 / totCount
                }).ToListAsync();


            return model;

        }

        public async Task<DashboardViewModel> GetDashboardData()
        {
            try
            {
                var dashboardData = new DashboardViewModel
                {
                    TotalORLogs = await _db.OverrideLogs.CountAsync(),
                    TotalWRRLogs = await _db.WRRLogs.CountAsync(),
                    TotalTotLogs = await _db.TOTLogs.CountAsync()
                };
                return dashboardData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetDashboardData() threw the following exception");
                return new DashboardViewModel();
            }
        }

        private IQueryable<TOTLog> GetFilteredTOTLogs(TOTLogSearchViewModel search)
        {
            return _db.TOTLogs.Where(x =>
                    (search.DelayType.Id == null || search.DelayType.Id == x.DelayTypeId)
                    &&
                    (search.Unit.Id == null || search.Unit.Id == x.UnitId)
            );
        }

        private IQueryable<OverrideLog> GetFilteredORLogs(TOTLogSearchViewModel search)
        {
            return _db.OverrideLogs.Where(x =>
                    search.Unit.Id == null || search.Unit.Id == 0 || search.Unit.Id == x.UnitId
                );
        }

        private IQueryable<WRRLog> GetFilteredWrrLogs(WRRLogSearchViewModel search)
        {
            return _db.WRRLogs.Where(x =>
                    (search.Department.Id == 0 || search.Department.Id == x.DepartmentId)
                    &&
                    (search.Unit.Id == 0 || search.Unit.Id == x.UnitId)
                );
        }
    }
}
