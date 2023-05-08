using Models.Common.Interfaces;

namespace ViewModels.OverrideLogs.ORLog
{
    public interface IORLogCost
    {
        List<ORLogCostViewModel> Costs { get; set; }
    }

    public interface IORLogCostDetailView : IORLogCost
    {
        long Id { get; set; }
    }
}
