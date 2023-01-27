using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.FolderService;
using TorranceApi.Controllers;
using ViewModels.Common;
using ViewModels.Shared;
using ViewModels.Shared.Folder;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FolderController : CrudBaseBriefController<FolderCreateViewModel, FolderModifyViewModel, FolderDetailViewModel, FolderDetailViewModel, FolderSearchViewModel>
    {
        public FolderController(IFolderService<FolderCreateViewModel, FolderModifyViewModel, FolderDetailViewModel> FolderService) : base(FolderService)
        {
        }
    }
}

