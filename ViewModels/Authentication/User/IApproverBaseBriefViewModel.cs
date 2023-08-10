namespace ViewModels
{
    public interface IApproverBaseBriefViewModel
    {
        string ErrorMessage { get; }
        bool IsValidationEnabled { get; }
        long? Id { get; set; }
        string? Name { get; set; }
    }
}