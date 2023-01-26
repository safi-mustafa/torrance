using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.File;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Shared;

namespace Repositories.Services.FolderService
{
    public class FolderService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Folder, CreateViewModel, UpdateViewModel, DetailViewModel>, IFolderService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, IFolderIcon, IFileModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IFolderIcon, IFileModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly IRepositoryResponse _response;

        public FolderService(ToranceContext db, ILogger<FolderService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IFileHelper fileHelper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _response = response;
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
        public override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            model.IconUrl = model.File != null ? _fileHelper.Save(model) : null;
            return base.Create(model);
        }

        public override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            model.IconUrl = model.File != null ? _fileHelper.Save(model) : model.IconUrl;
            return base.Update(model);
        }

    }
}
