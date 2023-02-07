using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices;
using ViewModels.OverrideLogs;
using ViewModels.Common;
using ViewModels.Authentication.Approver;
using Repositories.Services.CommonServices.ApprovalService.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin, Approver")]
    public class ApprovalController : CrudBaseBriefController<ApprovalDetailViewModel, ApprovalSearchViewModel>
    {
        public ApprovalController(IApprovalService permitTypeService) : base(permitTypeService)
        {
        }
    }
}

