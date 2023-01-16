using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

namespace ChargieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : TorranceController
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly ILogger<TimesheetController> _logger;


        public TimesheetController(
            ILogger<TimesheetController> logger,
            IMapper mapper, 
            IRepositoryResponse response)
        {
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

    }
}
