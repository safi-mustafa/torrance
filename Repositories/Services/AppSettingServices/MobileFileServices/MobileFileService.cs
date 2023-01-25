using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.File;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.AppSettings;
using Models.Common.Interfaces;
using Models.Common;
using Pagination;
using Repositories.Common;
using Repositories.Interfaces;
using Repositories.Services.CommonServices.ContractorService;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.Common.Contractor;
using System.Net.Mail;
using Repositories.Shared.AttachmentService;
using Enums;
using System.Net;

namespace Repositories.Services.AppSettingServices.MobileFileServices
{
    public class MobileFileService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<MobileFile, CreateViewModel, UpdateViewModel, DetailViewModel>, IMobileFileService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly IRepositoryResponse _response;
        private readonly IAttachmentService _attachment;

        public MobileFileService(ToranceContext db, ILogger<MobileFileService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IFileHelper fileHelper, IRepositoryResponse response, IAttachmentService attachment) : base(db, logger, mapper, response)
        {
            _db = db;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _response = response;
            _attachment = attachment;
        }

        public override Expression<Func<MobileFile, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as BaseFileSearchViewModel;
            return x => x.FileType == searchFilters.FileType;
        }

        [HttpPost]
        public override async Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            var viewModel = model as BaseFileUpdateViewModel;
            var attachment = new AttachmentVM
            {
                Name = viewModel.File.FileName,
                ExtensionType = Path.GetExtension(viewModel.File.FileName),
                File = viewModel.File,
                UploadDate = DateTime.Now,
                FileType = viewModel.FileType,
            };
            var result = await _attachment.CreateSingle(attachment);
            var check = result as RepositoryResponseWithModel<long>;
            var response = new RepositoryResponseWithModel<long> { ReturnModel = check.ReturnModel };
            return response;

            //viewModel.Url = viewModel.File != null ? _fileHelper.Save(viewModel) : null;
            //return await base.Create(model);
        }

        public override async Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            var viewModel = model as BaseFileUpdateViewModel;

            var attachment = new AttachmentVM
            {
                Id = viewModel.Id,
                Name = viewModel.File.FileName,
                ExtensionType = Path.GetExtension(viewModel.File.FileName),
                File = viewModel.File,
                UploadDate = DateTime.Now,
                FileType = viewModel.FileType,
            };
            var result = await _attachment.UpdateSingle(attachment);

            var response = new RepositoryResponseWithModel<long> { ReturnModel = viewModel.Id };
            return response;

            //viewModel.Url = viewModel.File != null ? _fileHelper.Save(viewModel) : null;
           // return await base.Update(model);
        }

    }
}
