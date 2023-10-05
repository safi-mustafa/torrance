namespace ViewModels.Dashboard
{
    public class TOTWorkDelayTypeChartViewModel
    {
        public List<LogPieChartViewModel> OngoingWorkDelay { get; set; }
        public List<LogPieChartViewModel> ReworkDelay { get; set; }
        public List<LogPieChartViewModel> ShiftDelay { get; set; }
        public List<LogPieChartViewModel> StartOfWorkDelay { get; set; }
    }
}