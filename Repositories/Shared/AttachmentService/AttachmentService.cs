using System.Linq.Expressions;
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
using ViewModels.Attachment;
using ViewModels.Shared;

namespace Repositories.Shared.AttachmentService
{
    public class AttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Attachment, CreateViewModel, UpdateViewModel, DetailViewModel>, IAttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IAttachmentUrl, IFileModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IAttachmentUrl, IFileModel, IIdentitifier, new()
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

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.Set<Attachment>().Include(x => x.Folder).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var result = _mapper.Map<DetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<DetailViewModel> { ReturnModel = result };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for {typeof(Attachment).FullName} in GetById()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for {typeof(Attachment).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public override Expression<Func<Attachment, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var search = filters as AttachmentSearchViewModel;
            return x => (search.Folder.Id == 0 || search.Folder.Id == x.FolderId);
        }
    }
}
