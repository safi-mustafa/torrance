using Repositories.Interfaces;
using ViewModels.Shared;

namespace Repositories.Services.FolderService
{
    public interface IFolderService : IBaseCrud<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel>
    {
    }
}
