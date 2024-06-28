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
        public List<ChartViewModel> Status { get; set; }
        public List<ChartViewModel> Requestor { get; set; }
        public List<ChartViewModel> AreaExecutionLead { get; set; }
        public List<ChartViewModel> BusinessTeamLeader { get; set; }
        public List<ChartViewModel> Company { get; set; }
    }
}
