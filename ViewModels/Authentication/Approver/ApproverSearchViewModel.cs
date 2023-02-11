using ViewModels.Authentication.User;
using ViewModels.Common.Unit;

namespace ViewModels.Authentication.Approver
{
    public class ApproverSearchViewModel : UserSearchViewModel
    {
        public UnitBriefViewModel Unit { get; set; } = new();
    }
}
