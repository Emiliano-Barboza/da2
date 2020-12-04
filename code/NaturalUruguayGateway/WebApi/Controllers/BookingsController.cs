using System;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using Microsoft.AspNetCore.Mvc;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.WebApi.Filters;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [Route("bookings")]
    public class BookingsController : BaseApiController
    {
        private readonly IBookingsService service;
        
        public BookingsController(IBookingsService service)
        {
            this.service = service;
        }
        
        [AuthorizationFilter(Roles = new [] { "Super admin" })]
        [Route("{id}/status")]
        [HttpPut]
        public async Task<IActionResult> UpdateBookingStatusAsync(int id, [FromBody] BookingStatus bookingStatusModel)
        {
            try
            {
                var booking = await service.UpdateBookingStatusAsync(id, bookingStatusModel);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{confirmationCode}/status")]
        [HttpGet]
        public async Task<IActionResult> GetBookingStatusAsync(string confirmationCode)
        {
            try
            {
                var booking = await service.GetBookingStatusAsync(confirmationCode);
                return Ok(booking);
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
        public async Task<IActionResult> GetBookingsAsync([FromQuery] PagingModel pagingModel)
        {
            try
            {
                PaginatedModel<Booking> paginatedBookings = await service.GetBookingsAsync(pagingModel);
                return Ok(paginatedBookings);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
    }
}