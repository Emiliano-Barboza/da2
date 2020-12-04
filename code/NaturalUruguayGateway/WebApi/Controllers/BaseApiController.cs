using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaturalUruguayGateway.WebApi.Exceptions;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected ObjectResult GetExceptionResponse(Exception exception, HttpRequest request)
        {
            var response = ExceptionResponseHandler.GetExceptionResponse(exception, request);
            return response;
        }
    }
}