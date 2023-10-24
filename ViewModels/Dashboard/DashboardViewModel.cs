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
        public List<ChartViewModel> ChartData { get; set; }
    }
    public class TOTPieChartViewModel : TOTWorkDelayTypeDetailChartViewModel
    {
        public List<ChartViewModel> Shift { get; set; }
        public List<ChartViewModel> Unit { get; set; }
        public List<ChartViewModel> Department { get; set; }
        public List<ChartViewModel> RequestReason { get; set; }
        public List<ChartViewModel> DelayTypeHours { get; set; }
        public List<ChartViewModel> DelayTypeCosts { get; set; }
    }


    public class OverridePieChartViewModel
    {
        public List<ChartViewModel> Shift { get; set; }
        public List<ChartViewModel> Unit { get; set; }
        public List<ChartViewModel> Department { get; set; }
        public List<ChartViewModel> RequestReason { get; set; }
    }

    public class WrrPieChartViewModel
    {
        public List<ChartViewModel> WeldMethods { get; set; }
        public List<ChartViewModel> RodTypes { get; set; }
    }

    public class ChartViewModel
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }

}
