﻿using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Pagination;
using Microsoft.AspNetCore.Authorization;
using Repositories.Shared.UserInfoServices;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.WeldingRodRecord;
using ViewModels.Authentication.Approver;
using Repositories.Services.WeldRodRecordServices.ApproverService;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApproverController : CrudBaseController<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel, ApproverDetailViewModel, ApproverSearchViewModel>
    {
        private readonly IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> _ApproverService;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public ApproverController(IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> ApproverService, IMapper mapper, IUserInfoService userInfoService) : base(ApproverService)
        {
            _ApproverService = ApproverService;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }
        [NonAction]
        public async override Task<IActionResult> Put([FromBody] ApproverModifyViewModel model)
        {
            return await base.Put(model);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put([FromBody] ApproverProfileModifyViewModel model)
        {
            var mappedModel = _mapper.Map<ApproverModifyViewModel>(model);
            return await base.Put(mappedModel);
        }

        [NonAction]
        public override Task<IActionResult> Post([FromBody] ApproverModifyViewModel model)
        {
            return base.Post(model);
        }
        [NonAction]
        public override Task<IActionResult> Delete(long id)
        {
            return base.Delete(id);
        }
    }
}
