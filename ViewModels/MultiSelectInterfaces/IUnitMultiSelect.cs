using ViewModels.Common.Unit;

namespace ViewModels.MultiSelectInterfaces
{
    public interface IUnitMultiSelect
    {
        List<long> UnitIds { get; set; }
        List<UnitBriefViewModel> Units { get; set; }
    }
}
