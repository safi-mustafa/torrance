using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.FolderService;
using System.Net;
using Torrance.Api.Controllers;
using ViewModels.Shared;
using ViewModels.Shared.Folder;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FolderController : CrudBaseBriefController<FolderDetailViewModel, FolderSearchViewModel>
    {
        private readonly IFolderService<FolderCreateViewModel, FolderModifyViewModel, FolderDetailViewModel> _folderService;

        public FolderController(IFolderService<FolderCreateViewModel, FolderModifyViewModel, FolderDetailViewModel> folderService, ILogger<FolderController> logger) : base(folderService, logger, "Folder")
        {
            _folderService = folderService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Get(long id)
        {
            var result = await _folderService.GetById(id);
            return ReturnProcessedResponse<FolderDetailViewModel>(result);
        }
    }
}

