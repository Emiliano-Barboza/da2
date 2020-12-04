using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Constants;
using NaturalUruguayGateway.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Services;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngine.Services;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Test
{
    [TestClass]
    public class BookingsServiceTest
    {
        private IFixture fixture = null;
        private Mock<IBookingsRepository> moqRepository = null;
        private Mock<ILodgmentCalculator> moqLodgmentCalculator = null;
        private Lodgment expectedLodgment = null;
        private Lodgment lodgment = null;
        private Booking expectedBooking = null;
        private BookingStatus expectedBookingStatus = null;
        private BookingConfirmationModel bookingConfirmationModel = null;
        private int bookingId = 0;
        private string bookingConfirmationCode = null;
        private BookingStatus bookingStatus = null;
        private BookingsService service = null;
        private double expectedPrice = 0;
        private IEnumerable<Booking> expectedBookings = null;
        private PagingModel paging = null;
        private PaginatedModel<Booking> paginatedBookings = null;

        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<BookingStatus>(c => c
                .Without(x => x.Bookings));
            fixture.Customize<Lodgment>(c => 
                c.Without(s => s.Spot)
                    .Without(y => y.Bookings)
                    .Without(y => y.Reviews));
            moqRepository = new Mock<IBookingsRepository>(MockBehavior.Strict);
            moqLodgmentCalculator = new Mock<ILodgmentCalculator>(MockBehavior.Strict);
            service = new BookingsService(moqRepository.Object, moqLodgmentCalculator.Object);
            expectedLodgment = fixture.Create<Lodgment>();
            lodgment = fixture.Create<Lodgment>();
            expectedBooking = fixture.Create<Booking>();
            expectedBooking.Lodgment.IsActive = true;
            bookingConfirmationModel = fixture.Create<BookingConfirmationModel>();
            bookingId = fixture.Create<int>();
            bookingConfirmationCode = fixture.Create<string>();
            bookingStatus = fixture.Create<BookingStatus>();
            expectedBookingStatus = fixture.Create<BookingStatus>();
            expectedPrice = fixture.Create<double>();
            expectedBookings = fixture.CreateMany<Booking>();
            paging = fixture.Create<PagingModel>();
            paginatedBookings = fixture.Create<PaginatedModel<Booking>>();
        }
        
        [TestMethod]
        public async Task UpdateBookingStatusAsync_BookingExistsAndBookingStatusExists_ShouldReturnTheBooking()
        {
            // Arrange
            moqRepository.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedBooking);
            moqRepository.Setup(x => x.GetBookingStatusByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedBookingStatus);
            moqRepository.Setup(x => x.UpdateBookingStatusAsync(It.IsAny<int>(), It.IsAny<BookingStatus>()))
                .ReturnsAsync(expectedBooking);

            // Act
            var actualBooking = await service.UpdateBookingStatusAsync(bookingId, bookingStatus);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedBooking, actualBooking);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task UpdateBookingStatusAsync_BookingNotExistsAndBookingStatusExists_ShouldThrownException()
        {
            // Arrange
            expectedBooking = null;
            moqRepository.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedBooking);

            // Act
            await service.UpdateBookingStatusAsync(bookingId, bookingStatus);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task UpdateBookingStatusAsync_BookingExistsAndBookingStatusNotExists_ShouldThrownException()
        {
            // Arrange
            expectedBookingStatus = null;
            moqRepository.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedBooking);
            moqRepository.Setup(x => x.GetBookingStatusByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedBookingStatus);

            // Act
            await service.UpdateBookingStatusAsync(bookingId, bookingStatus);
        }

        [TestMethod]
        public async Task GetBookingStatusAsync_NoRestrictions_ShouldReturnTheBooking()
        {
            // Arrange
            moqRepository.Setup(x => x.GetBookingStatusAsync(It.IsAny<string>())).ReturnsAsync(expectedBooking);
            // Act
            var actualBooking = await service.GetBookingStatusAsync(bookingConfirmationCode);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedBooking, actualBooking);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task GetBookingStatusAsync_BookingNotExists_ShouldReturnException()
        {
            // Arrange
            moqRepository.Setup(x => x.GetBookingStatusAsync(It.IsAny<string>())).ReturnsAsync((Booking) null);
            
            // Act
            await service.GetBookingStatusAsync(bookingConfirmationCode);
        }

        [TestMethod]
        public async Task AddBookingAtLodgmentAsync_LodgmentExist_ShouldReturnBookingConfirmedModel()
        {
            // Arrange
            moqRepository.Setup(x => x.AddBookingAsync(It.IsAny<Booking>()))
                .ReturnsAsync(expectedBooking);
            moqRepository.Setup(x => x.GetBookingStatusByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedBookingStatus);
            moqLodgmentCalculator.Setup(x => x.CalculateTotalStayAsync(
                    It.IsAny<LodgmentOptionsModel>(), It.IsAny<Lodgment>()))
                .ReturnsAsync(expectedPrice);

            // Act
            var actualBookingConfirmedModel =
                await service.AddBookingAtLodgmentAsync(lodgment, bookingConfirmationModel);

            //Assert
            moqRepository.VerifyAll();
            moqLodgmentCalculator.VerifyAll();
            Assert.AreEqual(expectedBooking, actualBookingConfirmedModel);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task AddBookingAtLodgmentAsync_BookingStatusNotExists_ShouldReturnException()
        {
            // Arrange
            expectedBookingStatus = null;
            moqRepository.Setup(x => x.AddBookingAsync(It.IsAny<Booking>()))
                .ReturnsAsync(expectedBooking);
            moqRepository.Setup(x => x.GetBookingStatusByNameAsync(BookingStatusName.Created))
                .ReturnsAsync(expectedBookingStatus);

            // Act
            await service.AddBookingAtLodgmentAsync(lodgment, bookingConfirmationModel);
        }

        [TestMethod]
        public async Task GetBookingsAsync_NoRestrictions_ShouldReturnTheBookings()
        {
            // Arrange
            paginatedBookings.Data = expectedBookings;
            paginatedBookings.Counts = new PaginatedCountModel(paging, expectedBookings.Count());
            moqRepository.Setup(x => x.GetBookingsAsync(It.IsAny<PagingModel>())).ReturnsAsync(paginatedBookings);
                
            // Act
            PaginatedModel<Booking> actualPaginatedBookings = await service.GetBookingsAsync(paging);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(paginatedBookings, actualPaginatedBookings);
        }
    }
}