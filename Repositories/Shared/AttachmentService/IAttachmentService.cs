﻿using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Helpers.File;
using Models.Common;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;

namespace Repositories.Shared.AttachmentService
{
    public interface IAttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IAttachmentUrl, IFileModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IAttachmentUrl, IFileModel, IIdentitifier, new()
    {
        // Task<IRepositoryResponse> DeleteByEntity(long entityId, AttachmentEntityType entityType);
    }
}
