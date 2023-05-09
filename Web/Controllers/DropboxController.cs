using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.AppSettingServices.DropboxServices;
using ViewModels.AppSettings.MobileFiles.Dropbox;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class DropboxController : CrudBaseController<DropboxModifyViewModel, DropboxModifyViewModel, DropboxDetailViewModel, DropboxDetailViewModel, DropboxSearchViewModel>
    {
        private readonly IDropboxService<DropboxModifyViewModel, DropboxModifyViewModel, DropboxDetailViewModel> _dropboxService;
        private readonly ILogger<DropboxController> _logger;

        public DropboxController(IDropboxService<DropboxModifyViewModel, DropboxModifyViewModel, DropboxDetailViewModel> dropboxService, ILogger<DropboxController> logger, IMapper mapper) : base(dropboxService, logger, mapper, "Dropbox", "Dropboxes")
        {
            _dropboxService = dropboxService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Url",data = "Url", orderable = true},
                new DataTableViewModel{title = "Status",data = "FormattedStatus", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }

        public async Task<ActionResult> UpdateLinkStatus()
        {
            try
            {
                var response = await _dropboxService.UpdateLinkStatus();
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Dropbox: Records altered Successfully at " + DateTime.UtcNow);
                    return Json(new
                    {
                        Success = true,
                        //ReloadDatatable = true,
                    });
                }
                return Json(false);
            }
            catch (Exception ex) { _logger.LogError($"Dropbox UpdateLinkStatus method threw an exception, Message: {ex.Message}"); return Json(false); }

        }
    }
}
