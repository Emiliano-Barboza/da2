using System;
using System.Linq;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories
{
    public class BookingsRepository : BaseRepository<Booking>, IBookingsRepository
    {
        private readonly DbSet<Booking> bookings;
        private readonly DbSet<BookingStatus> bookingsStatuses;

        public BookingsRepository(DbContext context) : base(context)
        {
            this.bookings = context.Set<Booking>();
            this.bookingsStatuses = context.Set<BookingStatus>();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            try
            {
                var booking = await bookings.FirstOrDefaultAsync(x => x.Id == id);
                return booking;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get booking by id async", e);
            }
        }

        public async Task<BookingStatus> GetBookingStatusByNameAsync(string name)
        {
            try
            {
                var bookingStatus = await bookingsStatuses.FirstOrDefaultAsync(x => x.Name == name);
                return bookingStatus;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get booking status by name async", e);
            }
        }

        public async Task<Booking> UpdateBookingStatusAsync(int id, BookingStatus bookingStatusModel)
        {
            try
            {
                var booking = await bookings.FirstOrDefaultAsync(x => x.Id == id);
                var bookingStatus = await bookingsStatuses.FirstOrDefaultAsync(x => x.Name == bookingStatusModel.Name);
            
                booking.BookingStatusId = bookingStatus.Id; 
                booking.StatusDescription = bookingStatusModel.Description ?? booking.StatusDescription; 
            
                await context.SaveChangesAsync();
                return booking;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get booking status by name async", e);
            }
        }

        public async Task<Booking> GetBookingStatusAsync(string confirmationCode)
        {
            try
            {
                var booking = await bookings.Include("BookingStatus")
                    .Include("Lodgment")
                    .FirstOrDefaultAsync(x => x.ConfirmationCode == confirmationCode);
                return booking;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get booking status async", e);
            }
        }
        
        public async Task<Booking> AddBookingAsync(Booking bookingModel)
        {
            try
            {
                var booking = await bookings.AddAsync(bookingModel);

                await context.SaveChangesAsync();
                return booking.Entity;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on add lodgment async", e);
            }
        }

        public async Task<PaginatedModel<Booking>> GetBookingsAsync(PagingModel pagingModel)
        {
            try
            {
                await Task.Yield();
                IQueryable<Booking> query = bookings.AsQueryable();
                if (!string.IsNullOrEmpty(pagingModel.FilterBy))
                {
                    query = query.Where(
                        x => x.Email.ToUpper().Contains(pagingModel.FilterBy.ToUpper()));
                }
                var paginated = await GetPaginatedElementsAsync(query, pagingModel, PrimaryKey);
                return paginated;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get bookings async", e);
            }
        }
    }
}