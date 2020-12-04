using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories
{
    public interface IBookingsRepository
    {
        Task<Booking> GetBookingByIdAsync(int id);
        Task<BookingStatus> GetBookingStatusByNameAsync(string name);
        Task<Booking> UpdateBookingStatusAsync(int id, BookingStatus bookingStatusModel);
        Task<Booking> GetBookingStatusAsync(string confirmationCode);
        Task<Booking> AddBookingAsync(Booking bookingModel);
        Task<PaginatedModel<Booking>> GetBookingsAsync(PagingModel pagingModel);
    }
}