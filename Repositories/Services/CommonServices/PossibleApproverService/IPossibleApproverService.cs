namespace Repositories.Services.CommonServices.PossibleApproverService
{
    public interface IPossibleApproverService
    {
        Task<string> GetPossibleApprovers(long? unitId, long? departmentId);
    }
}