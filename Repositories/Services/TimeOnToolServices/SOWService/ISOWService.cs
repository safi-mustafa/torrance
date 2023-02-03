﻿using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;
using ViewModels.TimeOnTools.SOW;


namespace Repositories.Services.TimeOnToolServices.SOWService
{
    public interface ISOWService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
