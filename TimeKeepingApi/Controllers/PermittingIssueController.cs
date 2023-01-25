using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ViewModels.TomeOnTools.PermittingIssue;
using Repositories.Services.TimeOnToolServices.PermittingIssueService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermittingIssueController : CrudBaseBriefController<PermittingIssueModifyViewModel, PermittingIssueModifyViewModel, PermittingIssueDetailViewModel, PermittingIssueDetailViewModel, PermittingIssueSearchViewModel>
    {
        public PermittingIssueController(IPermittingIssueService<PermittingIssueModifyViewModel, PermittingIssueModifyViewModel, PermittingIssueDetailViewModel> permitTypeService) : base(permitTypeService)
        {
        }
    }
}

