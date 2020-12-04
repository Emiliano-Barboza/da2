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
    public class LodgmentsControllerTest : BaseControllerTest
    {
        private Mock<ILodgmentsService> moqService = null;
        private LodgmentsController controller = null;
        private Lodgment expectedLodgment = null;
        private LodgmentOptionsModel lodgmentOptions = null;
        private int lodgmentId = 0;
        private BookingConfirmationModel bookingConfirmationModel;
        private Booking expectedBooking;
        private IEnumerable<Lodgment> expectedLodgments = null;
        private PaginatedModel<Lodgment> paginatedLodgments = null;
        private PagingModel paging = null;
        private FileModel fileModel = null;
        private List<string> expectedUrls = null;

        [TestInitialize]
        public void BeforeEach()
        {
            InitializeComponents();
            fixture.Customize<Lodgment>(c => c
                .Without(x => x.Spot)
                .Without((y => y.Reviews)));
            fixture.Customize<BookingStatus>(c => c
                .Without(x => x.Bookings));
            fixture.Customize<Booking>(c => c
                .Without(b => b.Lodgment));
            moqService = new Mock<ILodgmentsService>(MockBehavior.Strict);
            controller = new LodgmentsController(moqService.Object);
            controller.ControllerContext = controllerContext;
            expectedLodgment = fixture.Create<Lodgment>();
            lodgmentOptions = fixture.Create<LodgmentOptionsModel>();
            lodgmentId = fixture.Create<int>();
            bookingConfirmationModel = fixture.Create<BookingConfirmationModel>();
            expectedBooking = fixture.Create<Booking>();
            expectedLodgments = fixture.CreateMany<Lodgment>();
            paginatedLodgments = fixture.Create<PaginatedModel<Lodgment>>();
            paging = fixture.Create<PagingModel>();
            fileModel = fixture.Create<FileModel>();
            expectedUrls = fixture.CreateMany<string>().ToList();
        }
        
        [TestMethod]
        public async Task ActivateLodgmentAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.SetLodgmentActiveStatusAsync(It.IsAny<int>(), true)).ReturnsAsync(expectedLodgment);
            
            // Act
            var response = await controller.ActivateLodgmentAsync(lodgmentId) as ObjectResult;
            var actualLodgment = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [TestMethod]
        public async Task ActivateLodgmentAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.SetLodgmentActiveStatusAsync(It.IsAny<int>(), true)).Throws<Exception>();

            // Act
            var response = await controller.ActivateLodgmentAsync(lodgmentId);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task DeactivateLodgmentAsync_Success()
        {
            // Arrange
            moqService.Setup(x => x.SetLodgmentActiveStatusAsync(It.IsAny<int>(), false)).ReturnsAsync(expectedLodgment);
            
            // Act
            var response = await controller.DeactivateLodgmentAsync(lodgmentId) as ObjectResult;
            var actualLodgment = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [TestMethod]
        public async Task DeactivateLodgmentAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.SetLodgmentActiveStatusAsync(It.IsAny<int>(), false)).Throws<Exception>();

            // Act
            var response = await controller.DeactivateLodgmentAsync(lodgmentId);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }

        [TestMethod]
        public async Task AddBookingAtLodgmentAsync_Success()
        {
            //Arrange
            moqService.Setup(x => x.AddLodgmentBookingAsync(It.IsAny<int>(),
                    It.IsAny<BookingConfirmationModel>()))
                .ReturnsAsync(expectedBooking);
            
            //Act
            var response = await controller.AddLodgmentBookingAsync(lodgmentId, bookingConfirmationModel) as ObjectResult;
            var actualBooking = response?.Value as Booking;
            
            //Assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedBooking, actualBooking);
        }

        [TestMethod]
        public async Task AddBookingAtLodgmentAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            //Arrange
            moqService.Setup(x => x
                    .AddLodgmentBookingAsync(It.IsAny<int>(), It.IsAny<BookingConfirmationModel>()))
                .Throws<Exception>();
            
            //Act
            var response = await controller.AddLodgmentBookingAsync(lodgmentId, bookingConfirmationModel);
            
            //Assert
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetLodgmentAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>(), It.IsAny<LodgmentOptionsModel>())).ReturnsAsync(expectedLodgment);
            
            // Act
            var response = await controller.GetLodgmentAsync(lodgmentId) as ObjectResult;
            var actualLodgment = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [TestMethod]
        public async Task GetLodgmentAsync_WithLodgmentOptions_Success()
        {
            // Arrange
            moqService.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>(), It.IsAny<LodgmentOptionsModel>())).ReturnsAsync(expectedLodgment);
            
            // Act
            var response = await controller.GetLodgmentAsync(lodgmentId, lodgmentOptions) as ObjectResult;
            var actualLodgment = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [TestMethod]
        public async Task GetLodgmentAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>(), It.IsAny<LodgmentOptionsModel>())).Throws<Exception>();

            // Act
            var response = await controller.GetLodgmentAsync(lodgmentId);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetLodgmentsAsync_NoRestrictions_Success()
        {
            // Arrange
            paginatedLodgments.Data = expectedLodgments;
            paginatedLodgments.Counts = new PaginatedCountModel(paging, expectedLodgments.Count());
            moqService.Setup(x => 
                    x.GetLodgmentsAsync(It.IsAny<PagingModel>(), null, null, It.IsAny<bool>()))
                .ReturnsAsync(paginatedLodgments);
            
            // Act
            var response = await controller.GetLodgmentsAsync(paging) as ObjectResult;
            var actualPaginatedLodgments = response.Value as PaginatedModel<Lodgment>;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actualPaginatedLodgments, typeof(PaginatedModel<Lodgment>));
            Assert.AreEqual(expectedLodgments, actualPaginatedLodgments.Data);
            Assert.IsNotNull(actualPaginatedLodgments.Counts);
            Assert.AreEqual(paging, actualPaginatedLodgments.Counts.Paging);
            Assert.AreEqual(expectedLodgments.Count(), actualPaginatedLodgments.Counts.Total);
        }
        
        [TestMethod]
        public async Task GetLodgmentsAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => 
                x.GetLodgmentsAsync(It.IsAny<PagingModel>(), null,
                    null, It.IsAny<bool>())).Throws<Exception>();

            // Act
            var response = await controller.GetLodgmentsAsync(paging);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task DeleteLodgmentAsync_Success()
        {
            // Arrange
            moqService.Setup(x => x.DeleteLodgmentAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);
            
            // Act
            var response = await controller.DeleteLodgmentAsync(lodgmentId) as ObjectResult;
            var actualLodgment = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [TestMethod]
        public async Task DeleteLodgmentAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.DeleteLodgmentAsync(It.IsAny<int>())).Throws<Exception>();

            // Act
            var response = await controller.DeleteLodgmentAsync(lodgmentId);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task UploadImageAsync_Success()
        {
            // Arrange
            moqService.Setup(x => x.UploadImageAsync(It.IsAny<int>(), It.IsAny<FileModel>())).ReturnsAsync(expectedUrls);
            
            // Act
            var response = await controller.UploadImageAsync(lodgmentId, fileModel) as ObjectResult;
            var actualUrls = response.Value as List<string>;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            CollectionAssert.AreEqual(expectedUrls, actualUrls);
        }
        
        [TestMethod]
        public async Task UploadImageAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.UploadImageAsync(It.IsAny<int>(), It.IsAny<FileModel>())).Throws<Exception>();
            
            // Act
            var response = await controller.UploadImageAsync(lodgmentId, fileModel) as ObjectResult;
            var actualUrl = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
    }
}