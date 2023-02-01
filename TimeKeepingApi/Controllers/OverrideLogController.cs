
using System;
using Repositories.Services.CommonServices;
using Repositories.Services.OverrideLogServices.ORLogService;
using TorranceApi.Controllers;
using ViewModels.Common;
using ViewModels.OverrideLogs.ORLog;

namespace API.Controllers
{
    public class OverrideLogController : CrudBaseBriefController<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel, ORLogDetailViewModel, ORLogSearchViewModel>
    {
        public OverrideLogController(IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> ORLogService) : base(ORLogService)
        {
        }
    }
}

