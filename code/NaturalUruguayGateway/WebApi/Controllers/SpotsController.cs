using System;
using System.IO;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.WebApi.Filters;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [Route("spots")]
    public class SpotsController : BaseApiController
    {
        private readonly ISpotsService service;

        public SpotsController(ISpotsService service)
        {
            this.service = service;
        }

        [AuthorizationFilter(Roles = new [] { "Super admin" })]
        [Route("{id}/lodgments")]
        [HttpPost]
        public async Task<IActionResult> AddLodgmentToSpotAsync(int id, [FromBody] Lodgment lodgmentModel)
        {
            try
            {
                lodgmentModel.SpotId = id;
                var lodgment = await service.AddLodgmentToSpotAsync(lodgmentModel);
                return Ok(lodgment);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}/lodgments")]
        [HttpGet]
        public async Task<IActionResult> GetLodgmentsInSpotAsync(int id, 
            [FromQuery] LodgmentOptionsModel lodgmentOptionsModel, [FromQuery] PagingModel pagingModel)
        {
            try
            {
                PaginatedModel<Lodgment> paginatedLodgments  = await service.GetLodgmentsInSpotAsync(id, lodgmentOptionsModel, pagingModel);
                return Ok(paginatedLodgments);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [AuthorizationFilter(Roles = new [] { "Super admin" })]
        [Route("{id}/reports")]
        [HttpGet]
        public async Task<IActionResult> GetSpotReportAsync(int id, [FromQuery] ReportOptionsModel reportOptionsModel)
        {
            try
            {
                var report  = await service.GetSpotReportAsync(id, reportOptionsModel);
                return Ok(report);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetSpotAsync(int id)
        {
            try
            {
                var spot  = await service.GetSpotByIdAsync(id);
                return Ok(spot);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [AuthorizationFilterAttribute(Roles = new [] { "Super admin" })]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetSpotsAsync([FromQuery] PagingModel pagingModel)
        {
            try
            {
                PaginatedModel<Spot> paginatedLodgments  = await service.GetSpotsAsync(pagingModel);
                return Ok(paginatedLodgments);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [AuthorizationFilterAttribute(Roles = new [] { "Super admin" })]
        [Route("{id}/images")]
        [HttpPost]
        public async Task<IActionResult> UploadImageAsync(int id, [FromForm] FileModel fileModel)
        {
            try
            {
                var url  = await service.UploadImageAsync(id, fileModel);
                return Ok(url);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}/images/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> GetImageAsync(int id, string fileName)
        {
            try
            {
                Stream fileStream  = await service.GetImageAsync(id, fileName);
                if (fileStream == null)
                {
                    return NotFound();
                }
                return File(fileStream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
    }
}