using System;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.WebApi.Filters;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [Route("users")]
    [AuthorizationFilterAttribute(Roles = new [] { "Super admin" })]
    public class UsersController : BaseApiController
    {
        private readonly IUsersService service;
        
        public UsersController(IUsersService service)
        {
            this.service = service;
        }
        
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] PagingModel pagingModel)
        {
            try
            {
                PaginatedModel<User> paginatedUsers = await service.GetUsersAsync(pagingModel);
                return Ok(paginatedUsers);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            try
            {
                var user = await service.GetUserAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] User userModel)
        {
            try
            {
                var user = await service.AddUserAsync(userModel);
                return Ok(user);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var user = await service.DeleteUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] User userModel)
        {
            try
            {
                userModel.Id = id;
                var user = await service.UpdateUserAsync(userModel);
                return Ok(user);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
    }
}