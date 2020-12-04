using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaturalUruguayGateway.WebApi.Filters;
using NaturalUruguayGateway.ThridPartyImportInterface.Services;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [Route("imports")]
    public class ImportsController : BaseApiController
    {
        private readonly IImportsService service;
        
        public ImportsController(IImportsService service)
        {
            this.service = service;
        }
        
        [AuthorizationFilterAttribute(Roles = new [] { "Super admin" })]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetImports()
        {
            try
            {
                var imports = await service.GetImportsAsync();
                return Ok(imports);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
    }
}