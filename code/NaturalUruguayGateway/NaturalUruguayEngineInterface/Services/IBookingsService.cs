using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayEngineInterface.Services
{
    public interface IBookingsService
    {
        Task<Booking> UpdateBookingStatusAsync(int id,BookingStatus bookingStatusModel);
        Task<Booking> GetBookingStatusAsync(string confirmationCode);
        Task<Booking> AddBookingAtLodgmentAsync(Lodgment lodgment, BookingConfirmationModel bookingConfirmationModel);
        Task<PaginatedModel<Booking>> GetBookingsAsync(PagingModel pagingModel);
    }
}