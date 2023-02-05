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
        public List<LogPieChartViewModel> ShiftDelays { get; set; }
        public List<LogPieChartViewModel> ReworkDelays { get; set; }
    }

    public class WrrPieChartViewModel
    {
        public List<LogPieChartViewModel> WeldMethods { get; set; }
        public List<LogPieChartViewModel> RodTypes { get; set; }
    }
    public class LogPieChartViewModel
    {
        public string Category { get; set; }
        public float Value { get; set; }
    }

    public class BarChartViewModel
    {
        public string Category { get; set; }
        public float Value { get; set; }
    }

}
