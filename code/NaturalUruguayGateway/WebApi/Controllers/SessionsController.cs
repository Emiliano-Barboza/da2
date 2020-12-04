using System;
using System.Threading.Tasks;
using NaturalUruguayGateway.AuthorizationEngineInterface.Services;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [Route("sessions")]
    public class SessionsController : BaseApiController
    {
        private readonly ISessionsService service;
        
        public SessionsController(ISessionsService service)
        {
            this.service = service;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel login)
        {
            try
            {
                var session = await service.LoginAsync(login);

                return Ok(session);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
            
        }
        
        [Route("")]
        [HttpDelete]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                StringValues token = string.Empty;
                Request.Headers.TryGetValue("Authorization", out token);;
                var url = await service.LogoutAsync(token);

                return Ok(url);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
    }
}