using ViewModels.Dashboard;

namespace ViewModels
{
    public class TOTWorkDelayTypeDetailChartViewModel
    {
        public List<LogPieChartViewModel> OngoingWorkDelay { get; set; }
        public List<LogPieChartViewModel> ReworkDelay { get; set; }
        public List<LogPieChartViewModel> ShiftDelay { get; set; }
        public List<LogPieChartViewModel> StartOfWorkDelay { get; set; }
    }
}