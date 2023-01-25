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

        public MobileFileService(ToranceContext db, ILogger<MobileFileService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IFileHelper fileHelper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _response = response;
        }

        public override Expression<Func<MobileFile, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as BaseFileSearchViewModel;
            return x => x.Type == searchFilters.Type;
        }

        [HttpPost]
        public override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            var viewModel = model as BaseFileUpdateViewModel;
            viewModel.Url = viewModel.File != null ? _fileHelper.Save(viewModel) : null;
            return base.Create(model);
        }

        public override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            var viewModel = model as BaseFileUpdateViewModel;
            viewModel.Url = viewModel.File != null ? _fileHelper.Save(viewModel) : null;
            return base.Update(model);
        }

    }
}
