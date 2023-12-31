﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;
using ViewModels.DataTable;
using ViewModels.TimeOnTools.ShiftDelay;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class ShiftDelayController : CrudBaseController<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel, ShiftDelayDetailViewModel, ShiftDelaySearchViewModel>
    {
        private readonly IShiftDelayService<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel> _ShiftDelayService;
        private readonly ILogger<ShiftDelayController> _logger;

        public ShiftDelayController(IShiftDelayService<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel> ShiftDelayService, ILogger<ShiftDelayController> logger, IMapper mapper) : base(ShiftDelayService, logger, mapper, "ShiftDelay", "Shift Delay")
        {
            _ShiftDelayService = ShiftDelayService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }
    }
}
