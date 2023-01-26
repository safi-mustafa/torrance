﻿using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;

namespace Repositories.Services.FolderService
{
    public interface IFolderService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
