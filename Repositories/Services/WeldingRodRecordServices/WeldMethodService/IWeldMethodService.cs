﻿using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace Repositories.Services.AppSettingServices.WeldMethodService
{
    public interface IWeldMethodService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
