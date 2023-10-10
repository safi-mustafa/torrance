using ViewModels.Dashboard;

namespace ViewModels
{
    public class TOTWorkDelayTypeDetailChartViewModel
    {
        public List<ChartViewModel> OngoingWorkDelay { get; set; }
        public List<ChartViewModel> ReworkDelay { get; set; }
        public List<ChartViewModel> ShiftDelay { get; set; }
        public List<ChartViewModel> StartOfWorkDelay { get; set; }
    }
}