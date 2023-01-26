namespace Repositories.Shared.Interfaces
{
    public interface IBaseApprove
    {
        Task<List<long>> GetApprovedRecordIds();
        Task ApproveRecords(List<long> ids, bool Status);
    }
}
