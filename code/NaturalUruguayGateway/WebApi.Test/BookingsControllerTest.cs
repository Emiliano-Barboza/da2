using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NaturalUruguayGateway.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.WebApi.Controllers;

namespace NaturalUruguayGateway.WebApi.Test
{
    [TestClass]
    public class BookingsControllerTest : BaseControllerTest
    {   
        private Mock<IBookingsService> moqService = null;
        private BookingsController controller = null;
        private Booking expectedBooking = null;
        private BookingStatus bookingStatus = null;
        private int bookingId = 0;
        private string bookingConfirmationCode = null;
        private IEnumerable<Booking> expectedBookings = null;
        private PagingModel paging = null;
        private PaginatedModel<Booking> paginatedBookings = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            InitializeComponents();
            fixture.Customize<BookingStatus>(c => c
                .Without(x => x.Bookings));
            fixture.Customize<Booking>(c => c
                .Without(b => b.Lodgment));
            moqService = new Mock<IBookingsService>(MockBehavior.Strict);
            controller = new BookingsController(moqService.Object);
            controller.ControllerContext = controllerContext;
            expectedBooking = fixture.Create<Booking>();
            bookingId = fixture.Create<int>();
            bookingConfirmationCode = fixture.Create<string>();
            expectedBookings = fixture.CreateMany<Booking>();
            paging = fixture.Create<PagingModel>();
            paginatedBookings = fixture.Create<PaginatedModel<Booking>>();
        }
        
        [TestMethod]
        public async Task UpdateBookingStatusAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.UpdateBookingStatusAsync(It.IsAny<int>(), It.IsAny<BookingStatus>())).ReturnsAsync(expectedBooking);
            
            // Act
            var response = await controller.UpdateBookingStatusAsync(bookingId, bookingStatus) as ObjectResult;
            var actualBooking = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedBooking, actualBooking);
        }
        
        [TestMethod]
        public async Task UpdateBookingStatusAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.UpdateBookingStatusAsync(It.IsAny<int>(), It.IsAny<BookingStatus>())).Throws<Exception>();
            
            // Act
            var response = await controller.UpdateBookingStatusAsync(bookingId, bookingStatus);

            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetBookingStatusAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.GetBookingStatusAsync(It.IsAny<string>())).ReturnsAsync(expectedBooking);
            
            // Act
            var response = await controller.GetBookingStatusAsync(bookingConfirmationCode) as ObjectResult;
            var actualBooking = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedBooking, actualBooking);
        }
        
        [TestMethod]
        public async Task GetBookingStatusAsync_IfThrowsException_ShouldReturnBadRequest()
        {   
            // Arrange
            moqService.Setup(x => x.GetBookingStatusAsync(It.IsAny<string>())).Throws<Exception>();
            
            // Act
            var response = await controller.GetBookingStatusAsync(bookingConfirmationCode);

            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetBookingsAsync_NoRestrictions_Success()
        {
            // Arrange
            paginatedBookings.Data = expectedBookings;
            paginatedBookings.Counts = new PaginatedCountModel(paging, expectedBookings.Count());
            moqService.Setup(x => x.GetBookingsAsync(It.IsAny<PagingModel>())).ReturnsAsync(paginatedBookings);
            
            // Act
            var response = await controller.GetBookingsAsync(paging) as ObjectResult;
            var actualPaginatedBookings = response.Value as PaginatedModel<Booking>;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actualPaginatedBookings, typeof(PaginatedModel<Booking>));
            Assert.AreEqual(expectedBookings, actualPaginatedBookings.Data);
            Assert.IsNotNull(actualPaginatedBookings.Counts);
            Assert.AreEqual(paging, actualPaginatedBookings.Counts.Paging);
            Assert.AreEqual(expectedBookings.Count(), actualPaginatedBookings.Counts.Total);
        }
        
        [TestMethod]
        public async Task GetUsersAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.GetBookingsAsync(It.IsAny<PagingModel>())).Throws<Exception>();

            // Act
            var response = await controller.GetBookingsAsync(paging);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
    }
}