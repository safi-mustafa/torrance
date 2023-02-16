using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common.Validation;

namespace Repositories.Services.CommonServices.ValidationService.UniqueNameService
{
    public interface IBaseServiceWithUniqueNameValidation
    {
        Task<bool> IsUniqueName(IValidateName validateName);
    }
}
