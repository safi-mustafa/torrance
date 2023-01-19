﻿using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public interface ITOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
