using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.WebApi.Filters;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [Route("lodgments")]
    public class LodgmentsController : BaseApiController
    {
        private readonly ILodgmentsService service;

        public LodgmentsController(ILodgmentsService service)
        {
            this.service = service;
        }

        [AuthorizationFilter(Roles = new [] { "Super admin" })]
        [Route("{id}/activate")]
        [HttpPost]
        public async Task<IActionResult> ActivateLodgmentAsync(int id)
        {
            try
            {
                var lodgment = await service.SetLodgmentActiveStatusAsync(id, true);
                return Ok(lodgment);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [AuthorizationFilterAttribute(Roles = new [] { "Super admin" })]
        [Route("{id}/deactivate")]
        [HttpDelete]
        public async Task<IActionResult> DeactivateLodgmentAsync(int id)
        {
            try
            {
                var lodgment = await service.SetLodgmentActiveStatusAsync(id, false);
                return Ok(lodgment);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }

        [Route("{id}/bookings")]
        [HttpPost]
        public async Task<IActionResult> AddLodgmentBookingAsync(int id, [FromBody] BookingConfirmationModel bookingConfirmationModel)
        {
            try
            {
                var booking = await service.AddLodgmentBookingAsync(id, bookingConfirmationModel);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetLodgmentAsync(int id, [FromQuery] LodgmentOptionsModel lodgmentOptionsModel = null)
        {
            try
            {
                var lodgment = await service.GetLodgmentByIdAsync(id, lodgmentOptionsModel);
                return Ok(lodgment);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}/reviews")]
        [HttpGet]
        public async Task<IActionResult> GetLodgmentReviewsAsync(int id, [FromQuery] PagingModel pagingModel)
        {
            try
            {
                PaginatedModel<Review> paginatedReviews  = await service.GetLodgmentReviewsAsync(id, pagingModel);
                return Ok(paginatedReviews);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}/reviews")]
        [HttpPost]
        public async Task<IActionResult> AddLodgmentReviewAsync(int id, [FromBody] Review reviewModel)
        {
            try
            {
                Review review  = await service.AddLodgmentReviewAsync(id, reviewModel);
                return Ok(review);
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
        public async Task<IActionResult> GetLodgmentsAsync([FromQuery] PagingModel pagingModel)
        {
            try
            {
                PaginatedModel<Lodgment> paginatedLodgments  = await service.GetLodgmentsAsync(pagingModel, includeDeactivated:true);
                return Ok(paginatedLodgments);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [AuthorizationFilterAttribute(Roles = new [] { "Super admin" })]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLodgmentAsync(int id)
        {
            try
            {
                var lodgment = await service.DeleteLodgmentAsync(id);
                return Ok(lodgment);
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
                var urls  = await service.UploadImageAsync(id, fileModel);
                return Ok(urls);
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