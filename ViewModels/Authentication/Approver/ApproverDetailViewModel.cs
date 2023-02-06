using ViewModels.Common.Unit;

namespace ViewModels.Authentication.Approver
{
    public class ApproverDetailViewModel : UserDetailViewModel
    {
        public List<UnitBriefViewModel> Units { get; set; } = new List<UnitBriefViewModel>();
        public string FormattedUnits
        {
            get
            {
                return Units != null && Units.Count() > 0 ? string.Join(", ", Units.Select(m => m.Name).ToList()) : "";
            }
        }
    }
}
