using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public long TotalTotLogs { get; set; } = 0;
        public long TotalWRRLogs { get; set; } = 0;
        public long TotalORLogs { get; set; } = 0;
    }
    public class StatusChartViewModel
    {
        public List<BarChartViewModel> ChartData { get; set; }
    }
    public class TOTPieChartViewModel
    {
        public List<LogPieChartViewModel> Shift { get; set; }
        public List<LogPieChartViewModel> Unit { get; set; }
        public List<LogPieChartViewModel> Department { get; set; }
        public List<LogPieChartViewModel> RequestReason { get; set; }
        public List<LogPieChartViewModel> ShiftDelay { get; set; }
        public List<LogPieChartViewModel> ReworkDelay { get; set; }
        public List<LogPieChartViewModel> StartOfWorkDelay { get; set; }
        public List<LogPieChartViewModel> OngoingWorkDelay { get; set; }
    }


    public class OverridePieChartViewModel
    {
        public List<LogPieChartViewModel> Shift { get; set; }
        public List<LogPieChartViewModel> Unit { get; set; }
        public List<LogPieChartViewModel> Department { get; set; }
        public List<LogPieChartViewModel> RequestReason { get; set; }
    }

    public class WrrPieChartViewModel
    {
        public List<LogPieChartViewModel> WeldMethods { get; set; }
        public List<LogPieChartViewModel> RodTypes { get; set; }
    }

    public class LogPieChartViewModel
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }

    public class BarChartViewModel
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }

}
