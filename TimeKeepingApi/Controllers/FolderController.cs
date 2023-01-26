using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.FolderService;
using TorranceApi.Controllers;
using ViewModels.Common;
using ViewModels.Shared;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FolderController : CrudBaseBriefController<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel, FolderDetailViewModel, FolderSearchViewModel>
    {
        public FolderController(IFolderService<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel> FolderService) : base(FolderService)
        {
        }
    }
}

