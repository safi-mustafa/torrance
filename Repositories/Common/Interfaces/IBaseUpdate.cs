﻿using ViewModels.Shared;

namespace Repositories.Interfaces
{
    public interface IBaseUpdate<UpdateViewModel>
        where UpdateViewModel : class, IBaseCrudViewModel, new()
    {
        Task<long> Update(UpdateViewModel model);
    }

}
