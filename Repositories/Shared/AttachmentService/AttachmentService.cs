using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.File;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.AppSettings;
using Models.Common;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using Repositories.Services.FolderService;
using Repositories.Shared.AuthenticationService;
using ViewModels.Shared;

namespace Repositories.Shared.AttachmentService
{
    public class AttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Attachment, CreateViewModel, UpdateViewModel, DetailViewModel>, IAttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IAttachmentUrl, IFolderIcon, IFileModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IAttachmentUrl, IFolderIcon, IFileModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<AttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly IRepositoryResponse _response;
        private readonly UserManager<ToranceUser> _userManager;

        public AttachmentService(ToranceContext db, ILogger<AttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, UserManager<ToranceUser> userManager, IMapper mapper, IFileHelper fileHelper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _response = response;
            _userManager = userManager;
        }

        public override async Task<IRepositoryResponse> Create(CreateViewModel attachment)
        {
            attachment.Url = _fileHelper.Save(attachment);
            return await base.Create(attachment);
        }

        public async Task<IRepositoryResponse> Update(UpdateViewModel attachment)
        {
            if (attachment.File != null)
                attachment.Url = _fileHelper.Save(attachment);
            return await base.Update(attachment);
        }
    }

    public class IdComparer<T> : IEqualityComparer<T> where T : BaseDBModel
    {
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }
        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
