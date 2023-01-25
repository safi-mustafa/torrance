using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Repositories.Interfaces;
using ViewModels.Shared;

namespace Repositories.Shared.AttachmentService
{
    public interface IAttachmentService : IBaseSearch
    {
        Task<IRepositoryResponse> CreateMultiple(List<AttachmentVM> model);
        Task<IRepositoryResponse> CreateSingle(AttachmentVM attachment);
        Task<IRepositoryResponse> UpdateSingle(AttachmentVM attachment);
        Task<IRepositoryResponse> Update(List<AttachmentVM> model, long id);
        Task<IRepositoryResponse> Delete(List<long> attachmentIds);
        Task<IRepositoryResponse> DeleteByEntity(long entityId, AttachmentEntityType entityType);
    }
}
