using Centangle.Common.ResponseHelpers.Error;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using ViewModels.Shared;

namespace Torrance.Api.Controllers
{
    [Authorize]
    public class TorranceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly string _controllerName;
        public TorranceController(ILogger logger, string controllerName)
        {

            _logger = logger;
            _controllerName = controllerName;
        }
        protected IActionResult ReturnProcessedResponse(IRepositoryResponse response)
        {
            switch (response.Status)
            {
                case HttpStatusCode.OK:
                    {
                        return Ok(new { Status = 200, Message = response.Message });
                    }
                case HttpStatusCode.BadRequest:
                    {
                        if (ModelState.IsValid)
                            ErrorsHelper.AddErrorToModelState("", ErrorMessages.CommonError(), ModelState);
                        return BadRequest(new ValidationProblemDetails(ModelState));
                    }
                case HttpStatusCode.NotFound:
                    return NotFound();
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                default:
                    {
                        ErrorsHelper.AddErrorToModelState("", ErrorMessages.ProcessingError(), ModelState);
                        return BadRequest();
                    }
            }
        }

        protected IActionResult ReturnProcessedResponse<T>(IRepositoryResponse response) where T : new()
        {
            switch (response.Status)
            {
                case HttpStatusCode.OK:
                    {
                        var result = new ResponseModel<T> { Status = 200 };
                        if (response is IRepositoryResponseWithModel<T>)
                            result.Data = ((IRepositoryResponseWithModel<T>)response).ReturnModel;
                        return Ok(result);
                    }
                case HttpStatusCode.BadRequest:
                    {
                        if (ModelState.IsValid)
                            ErrorsHelper.AddErrorToModelState("", ErrorMessages.CommonError(), ModelState);

                        var badResponse = new
                        {
                            errors = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                        };
                        return BadRequest(new ValidationProblemDetails(ModelState));
                    }
                case HttpStatusCode.NotFound:
                    return NotFound();
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                default:
                    {
                        ErrorsHelper.AddErrorToModelState("", ErrorMessages.ProcessingError(), ModelState);
                        return BadRequest();
                    }
            }
        }

        protected virtual void LogModelStateError(object model)
        {
            var errors = "";
            foreach (var e in ModelState)
            {
                errors += $"{e.Key}: {e.Value}";
            }
            _logger.LogCritical($"{_controllerName} -> Post: {JsonConvert.SerializeObject(model)}, ModelStateError: {errors}");
        }
    }
}
