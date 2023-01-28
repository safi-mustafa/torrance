﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.FolderService;
using ViewModels.Shared;
using ViewModels.DataTable;
using Microsoft.AspNetCore.Mvc;
using Repositories.Shared.AttachmentService;
using Centangle.Common.ResponseHelpers.Models;
using Pagination;

namespace Web.Controllers
{
    [Authorize]

    public class FolderController : CrudBaseController<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel, FolderDetailViewModel, FolderSearchViewModel>
    {
        private readonly IFolderService<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel> _folderService;
        private readonly ILogger<FolderController> _logger;

        public FolderController
            (
                IFolderService<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel> folderService,
                ILogger<FolderController> logger,
                IMapper mapper
            ) : base(folderService, logger, mapper, "Folder", "Folders")
        {
            _folderService = folderService;
            _logger = logger;
        }

        public override ActionResult Index()
        {
            try
            {
                var model = (SearchFolders(new FolderSearchViewModel { DisablePagination = true })).Result;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Folder _GetFolderView method threw an exception, Message: {ex.Message}");
            }
            return null;
        }
        [HttpPost]
        public async Task<List<FolderDetailViewModel>> SearchFolders(FolderSearchViewModel model)
        {
            var response = await _folderService.GetAll<FolderDetailViewModel>(model);
            var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<FolderDetailViewModel>>;
            return parsedResponse?.ReturnModel.Items ?? new List<FolderDetailViewModel>();
        }

        public async Task<IActionResult> _GetFolders(FolderSearchViewModel search)
        {
            var response = await SearchFolders(search);
            return View(response);
        }

        public override List<DataTableViewModel> GetColumns()
        {
            throw new NotImplementedException();
        }
    }
}
