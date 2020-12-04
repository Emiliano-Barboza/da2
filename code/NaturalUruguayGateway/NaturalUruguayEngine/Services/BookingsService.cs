using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Constants;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Services;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IBookingsRepository repository;
        private readonly ILodgmentCalculator lodgmentCalculator;

        public BookingsService(IBookingsRepository repository, ILodgmentCalculator lodgmentCalculator)
        {
            this.repository = repository;
            this.lodgmentCalculator = lodgmentCalculator;
        }
        
        private async Task<Booking> CreateBookingAsync(Lodgment lodgment, BookingConfirmationModel bookingConfirmationModel)
        {
            var bookingStatus = await repository.GetBookingStatusByNameAsync(BookingStatusName.Created);
            if (bookingStatus == null)
            {
                throw new KeyNotFoundException($"Cannot find the booking status with name {BookingStatusName.Created}");
            }

            var amountGuest = (byte) bookingConfirmationModel.LodgmentOptions.TotalGuests;
            var price = await lodgmentCalculator.CalculateTotalStayAsync(
                bookingConfirmationModel.LodgmentOptions, lodgment
            );
            var booking = new Booking()
            {
                CheckIn = bookingConfirmationModel.LodgmentOptions.CheckIn,
                CheckOut = bookingConfirmationModel.LodgmentOptions.CheckOut,
                AmountGuest = amountGuest,
                Name = bookingConfirmationModel.Name,
                LastName = bookingConfirmationModel.LastName,
                Email = bookingConfirmationModel.Email,
                ConfirmationCode = Guid.NewGuid().ToString(),
                Price = price,
                BookingStatusId = bookingStatus.Id,
                LodgmentId = lodgment.Id,
                StatusDescription = BookingStatusName.Created
            };

            return booking;
        }

        public async Task<Booking> UpdateBookingStatusAsync(int id, BookingStatus bookingStatusModel)
        {
            var booking = await repository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                throw new KeyNotFoundException("La reserva no existe.");
            }

            var bookingStatus = await repository.GetBookingStatusByNameAsync(bookingStatusModel.Name);
            if (bookingStatus == null)
            {
                throw new KeyNotFoundException("El estado de reserva no existe.");
            }

            booking = await repository.UpdateBookingStatusAsync(id, bookingStatusModel);
            return booking;
        }

        public async Task<Booking> GetBookingStatusAsync(string confirmationCode)
        {
            var booking = await repository.GetBookingStatusAsync(confirmationCode);
            if (booking == null)
            {
                throw new KeyNotFoundException("La reserva no existe.");
            }
            
            return booking;
        }
        
        public async Task<Booking> AddBookingAtLodgmentAsync(Lodgment lodgment, BookingConfirmationModel bookingConfirmationModel)
        {   
            var bookingModel = await CreateBookingAsync(lodgment, bookingConfirmationModel);
            var booking = await repository.AddBookingAsync(bookingModel);
            return booking;
        }

        public async Task<PaginatedModel<Booking>> GetBookingsAsync(PagingModel pagingModel)
        {
            PaginatedModel<Booking> paginatedBookings = await repository.GetBookingsAsync(pagingModel);
            return paginatedBookings;
        }
    }
}