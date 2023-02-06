namespace ViewModels.MultiSelectInterfaces
{
    public interface ICraftMultiSelect
    {
        List<long> CraftIds { get; set; }
        List<BaseBriefVM> Crafts { get; set; }
    }
}
