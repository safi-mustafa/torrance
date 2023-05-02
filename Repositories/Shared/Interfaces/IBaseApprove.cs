using Centangle.Common.ResponseHelpers.Models;
using Enums;

namespace Repositories.Shared.Interfaces
{
    public interface IBaseApprove
    {
        Task<List<long>> GetApprovedRecordIds();
        Task ApproveRecords(List<long> ids, bool Status);
        Task<IRepositoryResponse> SetApproveStatus(long id, Status status, bool isUnauthenticatedApproval = false, long approverId = 0, Guid notificationId = new Guid(), string requestorEmail = "");
    }
}
