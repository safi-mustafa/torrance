using Repositories.Interfaces;
using ViewModels.TomeOnTools.PermitType;

namespace Repositories.Services.TimeOnToolServices.PermitTypeService
{
    public interface IPermitTypeService : IBaseCrud<PermitTypeModifyViewModel, PermitTypeModifyViewModel, PermitTypeDetailViewModel>
    {
    }
}
