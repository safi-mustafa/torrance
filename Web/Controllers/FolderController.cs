using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.FolderService;
using ViewModels.Shared;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices.MobileFileServices;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Shared.Folder;

namespace Web.Controllers
{
    [Authorize]

    public class FolderController : CrudBaseController<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel, FolderDetailViewModel, FolderSearchViewModel>
    {
        private readonly IFolderService<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel> _folderService;
        private readonly ILogger<FolderController> _logger;

        public FolderController(IFolderService<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel> folderService, ILogger<FolderController> logger, IMapper mapper) : base(folderService, logger, mapper, "Folder", "Folders")
        {
            _folderService = folderService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name"},
                new DataTableViewModel{title = "Status",data = "FormattedStatus"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }


        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> _GetAttachmentView(long id)
        {
            try
            {
                var model = await _folderService.GetFolderAttachments(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Folder _GetAttachmentView method threw an exception, Message: {ex.Message}");
            }
            return null;
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> _GetFolderView(long employeeId)
        {
            try
            {
                var model = await _folderService.GetFolders(employeeId);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Folder _GetFolderView method threw an exception, Message: {ex.Message}");
            }
            return null;
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateAttachment(FolderModifyViewModel model)
        {
            try
            {
                var folderId = await _folderService.CreateAttachments(model);
                return RedirectToAction("_GetAttachmentView", new { id = model.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Folder CreateAttachment method threw an exception, Message: {ex.Message}");
            }
            return null;
        }

    }
}
