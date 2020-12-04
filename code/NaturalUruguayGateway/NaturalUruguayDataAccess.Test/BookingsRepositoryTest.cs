using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.DataAccess.Context;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Test
{
    [TestClass]
    public class BookingsRepositoryTest
    {
        private IFixture fixture = null;
        private DbContextOptionsBuilder<NaturalUruguayContext> contextBuilderOptions = null;
        private NaturalUruguayContext context = null;
        private IEnumerable<Booking> expectedBookings = null;
        private Booking expectedBooking = null;
        private BookingStatus expectedBookingStatus = null;
        private BookingStatus bookingStatus = null;
        private int bookingId = 0;
        private string bookingConfirmationCode = null;
        private string bookingStatusName = null;
        private IBookingsRepository repository = null;
        private PagingModel paging = null;
        private const string DataBaseName = "NaturalUruguayDB";

        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<Booking>(c => c
                .Without(b => b.Lodgment));
            fixture.Customize<BookingStatus>(c => c
                .Without(bs => bs.Bookings));
            fixture.Customize<Lodgment>(c => c
                .Without(l => l.Spot)
                .Without(l => l.Reviews)
                .Without(l => l.Bookings));
            expectedBookings = fixture.CreateMany<Booking>();
            expectedBooking = fixture.Create<Booking>();
            expectedBookingStatus = fixture.Create<BookingStatus>();
            bookingStatus = fixture.Create<BookingStatus>();
            bookingId = fixture.Create<int>();
            bookingConfirmationCode = fixture.Create<string>();
            bookingStatusName = fixture.Create<string>();
            contextBuilderOptions = new DbContextOptionsBuilder<NaturalUruguayContext>();
            contextBuilderOptions.UseInMemoryDatabase(databaseName: DataBaseName);
            context = new NaturalUruguayContext(contextBuilderOptions.Options);
            context.Database.EnsureDeleted();
            
            repository = new BookingsRepository(context);
        }
        
        [TestMethod]
        public async Task GetBookingByIdAsync_BookingExists_ShouldReturnTheBooking()
        {
            // Arrange 
            expectedBooking.Id = bookingId;
            context.Add(expectedBooking);
            context.SaveChanges();
            
            //Act
            var actualBooking = await repository.GetBookingByIdAsync(bookingId);

            //Assert
            Assert.AreEqual(expectedBooking, actualBooking);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetBookingByIdAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetBookingByIdAsync(bookingId);
        }
        
        [TestMethod]
        public async Task GetBookingStatusByNameAsync_BookingExists_ShouldReturnTheBookingStatus()
        {
            // Arrange 
            expectedBookingStatus.Name = bookingStatusName;
            context.Add(expectedBookingStatus);
            context.SaveChanges();
            
            //Act
            var actualBookingStatus = await repository.GetBookingStatusByNameAsync(bookingStatusName);

            //Assert
            Assert.AreEqual(expectedBookingStatus, actualBookingStatus);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetBookingStatusByNameAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetBookingStatusByNameAsync(bookingStatusName);
        }
        
        [TestMethod]
        public async Task UpdateBookingStatusAsync_BookingExistsBookingStatusWithDescription_ShouldReturnTheBooking()
        {
            // Arrange
            expectedBooking.Id = bookingId;
            context.Add(expectedBookingStatus);
            context.Add(expectedBooking);
            context.SaveChanges();
            
            //Act
            var actualBooking = await repository.UpdateBookingStatusAsync(bookingId, expectedBookingStatus);

            //Assert
            Assert.AreEqual(expectedBookingStatus.Id, actualBooking.BookingStatusId);
            Assert.AreEqual(expectedBookingStatus.Description, actualBooking.StatusDescription);
        }
        
        [TestMethod]
        public async Task UpdateBookingStatusAsync_BookingExistsBookingStatusWithoutDescription_ShouldReturnTheBooking()
        {
            // Arrange
            expectedBookingStatus.Description = null;
            expectedBooking.Id = bookingId;
            context.Add(expectedBookingStatus);
            context.Add(expectedBooking);
            context.SaveChanges();
            
            //Act
            var actualBooking = await repository.UpdateBookingStatusAsync(bookingId, expectedBookingStatus);

            //Assert
            Assert.AreEqual(expectedBookingStatus.Id, actualBooking.BookingStatusId);
            Assert.AreNotEqual(expectedBookingStatus.Description, actualBooking.StatusDescription);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task UpdateBookingStatusAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.UpdateBookingStatusAsync(bookingId, bookingStatus);
        }
        
        [TestMethod]
        public async Task GetBookingStatusAsync_NoRestrictions_ShouldReturnTheBooking()
        {
            // Arrange
            expectedBooking.BookingStatusId = bookingStatus.Id;
            expectedBooking.BookingStatus = bookingStatus;
            expectedBooking.ConfirmationCode = bookingConfirmationCode;
            expectedBooking.Lodgment = fixture.Create<Lodgment>();
            context.Add(bookingStatus);
            context.Add(expectedBooking);
            context.SaveChanges();
            
            //Act
            var actualBooking = await repository.GetBookingStatusAsync(bookingConfirmationCode);

            //Assert
            Assert.AreEqual(expectedBooking, actualBooking);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetBookingStatusAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetBookingStatusAsync(bookingConfirmationCode);
        }
        
        [TestMethod]
        public async Task AddBookingAsync_NoRestrictions_ShouldReturnTheBooking()
        {
            // Act
            var actualBooking = await repository.AddBookingAsync(expectedBooking);

            // Assert
            Assert.AreEqual(expectedBooking.Id, actualBooking.Id);
        }

        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task AddBookingAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            // Act
            await repository.AddBookingAsync(expectedBooking);
        }
        [TestMethod]
        public async Task GetBookingsAsync_ThereAreUsers_ShouldReturnThePaginatedUsers()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            context.Add(expectedBooking);
            await context.SaveChangesAsync();
            
            //Act
            var actualPaginatedUsers = await repository.GetBookingsAsync(paging);

            //Assert
            Assert.AreEqual(expectedBooking, actualPaginatedUsers.Data.First());
        }
        
        [TestMethod]
        public async Task GetBookingsAsync_UserExistsFilterBy_ShouldReturnThePaginatedUsers()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            
            paging.FilterBy = expectedBooking.Email;
            context.Add(expectedBooking);
            context.SaveChanges();
            
            //Act
            var actualPaginatedBookings = await repository.GetBookingsAsync(paging);

            //Assert
            Assert.IsTrue(actualPaginatedBookings.Data.Count() == 1);
            Assert.IsTrue(actualPaginatedBookings.Counts.Total == 1);
            Assert.AreEqual(expectedBooking, actualPaginatedBookings.Data.First());
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetBookingsAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange 
            await context.DisposeAsync();
            
            //Act
            await repository.GetBookingsAsync(paging);
        }
    }
}