using DataLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Dashboard;

namespace Repositories.Services.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly ToranceContext _db;

        public DashboardService(ToranceContext db)
        {
            _db = db;
        }

        public async Task<TOTPieChartViewModel> GetTotChartsData()
        {
            var model = new TOTPieChartViewModel();
            var totCount = await GetFilteredTOTLogs().CountAsync();
            model.ShiftDelays = await GetFilteredTOTLogs()
              .Include(x => x.ShiftDelay)
              .GroupBy(x => x.ShiftDelayId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.ShiftDelay.Name),
                  Value = x.Count() * 100 / totCount
              }).ToListAsync();

            model.ReworkDelays = await GetFilteredTOTLogs()
                .Include(x => x.ReworkDelay)
                .GroupBy(x => x.ReworkDelayId).Select(x => new LogPieChartViewModel
                {
                    Category = x.Max(y => y.ReworkDelay.Name),
                    Value = x.Count() * 100 / totCount
                }).ToListAsync();


            return model;

        }

        public async Task<WrrPieChartViewModel> GetWrrChartsData()
        {
            var model = new WrrPieChartViewModel();
            var totCount = await GetFilteredWrrLogs().CountAsync();
            model.WeldMethods = await GetFilteredWrrLogs()
              .Include(x => x.WeldMethod)
              .GroupBy(x => x.WeldMethodId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.WeldMethod.Name),
                  Value = x.Count() * 100 / totCount
              }).ToListAsync();

            model.RodTypes = await GetFilteredWrrLogs()
                .Include(x => x.RodType)
                .GroupBy(x => x.RodTypeId).Select(x => new LogPieChartViewModel
                {
                    Category = x.Max(y => y.RodType.Name),
                    Value = x.Count() * 100 / totCount
                }).ToListAsync();


            return model;

        }

        private IQueryable<TOTLog> GetFilteredTOTLogs()
        {
            return _db.TOTLogs;
        }

        private IQueryable<WRRLog> GetFilteredWrrLogs()
        {
            return _db.WRRLogs;
        }
    }
}
