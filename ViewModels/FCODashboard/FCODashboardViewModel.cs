using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Dashboard;

namespace ViewModels.FCODashboard
{
    public class FCODashboardViewModel
    {

    }

    public class FCOPieChartViewModel
    {
        public List<LogPieChartViewModel> Status { get; set; }
        public List<LogPieChartViewModel> Requestor { get; set; }
        public List<LogPieChartViewModel> Approver { get; set; }
        public List<LogPieChartViewModel> Company { get; set; }
    }
}
