using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ViewModels.Authentication.Approver;
using Web.Controllers.SharedControllers;
using AutoMapper;
using Repositories.Services.WeldRodRecordServices.ApproverService;
using ViewModels.DataTable;
using Repositories.Services.CommonServices.ApprovalService.Interface;
using ViewModels.Common;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ApprovalController : DatatableBaseController<ApprovalDetailViewModel, ApprovalSearchViewModel>
    {
        private readonly IApprovalService _approvalService;
        private readonly ILogger<ApprovalController> _logger;

        public ApprovalController(IApprovalService approvaleService, ILogger<ApprovalController> logger, IMapper mapper) : base(approvaleService, logger, mapper, "Approval", "Approvals")
        {
            _approvalService = approvaleService;
            _logger = logger;
        }


        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/Approval/Detail/Id"},
                    new DataTableActionViewModel() {Action="Update",Title="Update",Href=$"/Approval/Update/Id"},
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/Approval/Delete/Id"},
            };
        }
        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Status",data = "Status"},
                new DataTableViewModel{title = "Date",data = "Date"},
                new DataTableViewModel{title = "Type",data = "Type"},
                new DataTableViewModel{title = "Requester",data = "Requester"},
                new DataTableViewModel{title = "Department",data = "Department"},
                new DataTableViewModel{title = "Contractor",data = "Contractor"},
                new DataTableViewModel{title = "Unit",data = "Unit"},
                new DataTableViewModel{title = "TWR",data = "TWR"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}

