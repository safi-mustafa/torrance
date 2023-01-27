using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.File;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using Repositories.Shared.AttachmentService;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.Shared.Folder;

namespace Repositories.Services.FolderService
{
    public class FolderService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Folder, CreateViewModel, UpdateViewModel, DetailViewModel>, IFolderService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IFolderIcon, IFileModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IFolderIcon, IFileModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<FolderService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly IRepositoryResponse _response;
        private readonly IAttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel> _attachmentService;

        public FolderService
            (
                ToranceContext db, 
                ILogger<FolderService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, 
                IMapper mapper, 
                IFileHelper fileHelper, 
                IRepositoryResponse response,
                IAttachmentService<CreateViewModel, UpdateViewModel, DetailViewModel> attachmentService
            ) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _response = response;
            _attachmentService = attachmentService;
        }

        public override Expression<Func<Folder, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as FolderSearchViewModel;

            return x => (
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                            &&
                            (searchFilters.Status == null || x.ActiveStatus == searchFilters.Status)
                        );
        }

        [HttpPost]
        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            model.IconUrl = model.File != null ? _fileHelper.Save(model) : null;
            return await base.Create(model);
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            model.IconUrl = model.File != null ? _fileHelper.Save(model) : model.IconUrl;
            return await base.Update(model);
        }

        public async override Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.Set<Folder>().Include(x => x.Attachments).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var result = _mapper.Map<DetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<DetailViewModel> { ReturnModel = result };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for Folder in GetById()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for Folder threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        //public async Task<List<FolderDetailViewModel>> GetFolders(long id)
        //{
        //    try
        //    {
        //        var folders = await _db.Folders.Where(x => (x.Id == id) && (x.ActiveStatus == ActiveStatus.Active)).ToListAsync();
        //        if (folders.Count > 0)
        //        {
        //            var mappedFolders = _mapper.Map<List<FolderDetailViewModel>>(folders);
        //            foreach (var folder in mappedFolders)
        //            {
        //                var attachments = await _db.Attachments.Where(x => x.EntityId == folder.Id && x.EntityType == Enums.AttachmentEntityType.Folder && x.UserId == id).ToListAsync();
        //                var mappedAttachments = _mapper.Map<List<AttachmentVM>>(attachments);
        //                folder.Attachments.AddRange(mappedAttachments);
        //            }

        //            return mappedFolders.OrderBy(x => x.Name).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;
        //}

        public async Task<long> CreateAttachments(CreateViewModel model)
        {
            try
            {
                //model.Attachments.ForEach(x =>
                //{
                //    x.Folder = new FolderBriefViewModel { Id = model.Id};
                //});
                var folderId = await _attachmentService.Create(model);
                return 0;
            }
            catch (Exception ex)
            {

            }
            return -1;
        }

        public async Task<FolderViewModel> GetFolderAttachments(long id)
        {
            try
            {
                var attachments = await _db.Attachments.Where(x => x.EntityId == id).ToListAsync();
                var mappedAttachments = _mapper.Map<List<AttachmentVM>>(attachments);
                FolderViewModel viewModel = new()
                {
                    Id = id,
                    Attachments = mappedAttachments,
                };
                return viewModel;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<FolderDetailViewModel> GetFolders(long id)
        {
            try
            {
                var folders = await GetById(id);
                return new FolderDetailViewModel();
             //   return folders;

            }
            catch (Exception ex)
            {

            }
            return null;
        }

    }
}
