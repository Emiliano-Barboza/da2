using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.WebApi.Controllers;

namespace NaturalUruguayGateway.WebApi.Test
{
    [TestClass]
    public class SpotsControllerTest : BaseControllerTest
    {
        private Mock<ISpotsService> moqService = null;
        private SpotsController controller = null;
        private Lodgment expectedLodgment = null;
        private IEnumerable<Lodgment> expectedLodgments = null;
        private Lodgment lodgment = null;
        private LodgmentOptionsModel lodgmentOptions = null;
        private PaginatedModel<Lodgment> paginatedLodgments = null;
        private PagingModel paging = null;
        private int lodgmentId = 0;
        private int spotId = 0;
        private Spot expectedSpot = null;
        private IEnumerable<Spot> expectedSpots = null;
        private PaginatedModel<Spot> paginatedSpots = null;

        [TestInitialize]
        public void BeforeEach()
        {
            InitializeComponents();
            fixture.Customize<Lodgment>(c => c
                .Without(x => x.Spot)
                .Without(y => y.Reviews)
                .Without(y => y.Bookings));
            fixture.Customize<Spot>(c => c
                .Without(x => x.Region));
            fixture.Customize<CategorySpot>(c => c
                .Without(x => x.Category)
                .Without(x => x.Spot));
            moqService = new Mock<ISpotsService>(MockBehavior.Strict);
            controller = new SpotsController(moqService.Object);
            controller.ControllerContext = controllerContext;
            expectedLodgment = fixture.Create<Lodgment>();
            expectedLodgments = fixture.CreateMany<Lodgment>();
            lodgment = fixture.Create<Lodgment>();
            lodgmentOptions = fixture.Create<LodgmentOptionsModel>();
            paginatedLodgments = fixture.Create<PaginatedModel<Lodgment>>();
            paging = fixture.Create<PagingModel>();
            lodgmentId = fixture.Create<int>();
            spotId = fixture.Create<int>();
            expectedSpot = fixture.Create<Spot>();
            expectedSpots = fixture.CreateMany<Spot>();
            paginatedSpots = fixture.Create<PaginatedModel<Spot>>();
        }

        [TestMethod]
        public async Task AddLodgmentToSpotAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.AddLodgmentToSpotAsync(It.IsAny<Lodgment>())).ReturnsAsync(expectedLodgment);
            
            // Act
            var response = await controller.AddLodgmentToSpotAsync(spotId, lodgment) as ObjectResult;
            var actualLodgment = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [TestMethod]
        public async Task AddLodgmentToSpotAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.AddLodgmentToSpotAsync(It.IsAny<Lodgment>())).Throws<Exception>();

            // Act
            var response = await controller.AddLodgmentToSpotAsync(spotId, lodgment);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }

        [TestMethod]
        public async Task GetLodgmentsInSpotAsync_NoRestrictions_Success()
        {
            // Arrange
            paginatedLodgments.Data = expectedLodgments;
            paginatedLodgments.Counts = new PaginatedCountModel(paging, expectedLodgments.Count());
            moqService.Setup(x => 
                x.GetLodgmentsInSpotAsync(It.IsAny<int>(), It.IsAny<LodgmentOptionsModel>(), It.IsAny<PagingModel>()))
                .ReturnsAsync(paginatedLodgments);
            
            // Act
            var response = await controller.GetLodgmentsInSpotAsync(spotId, lodgmentOptions, paging) as ObjectResult;
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
        public async Task GetLodgmentsInSpotAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => 
                x.GetLodgmentsInSpotAsync(It.IsAny<int>(), It.IsAny<LodgmentOptionsModel>(), 
                    It.IsAny<PagingModel>())).Throws<Exception>();

            // Act
            var response = await controller.GetLodgmentsInSpotAsync(spotId, lodgmentOptions, paging);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetSpotAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => 
                    x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSpot);
            
            // Act
            var response = await controller.GetSpotAsync(spotId) as ObjectResult;
            var actualSpot = response.Value as Spot;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [TestMethod]
        public async Task GetSpotAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => 
                x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSpot);
            
            // Act
            var response = await controller.GetSpotAsync(spotId) as ObjectResult;

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetSpotsAsync_NoRestrictions_Success()
        {
            // Arrange
            paginatedSpots.Data = expectedSpots;
            paginatedSpots.Counts = new PaginatedCountModel(paging, expectedSpots.Count());
            moqService.Setup(x => 
                    x.GetSpotsAsync(It.IsAny<PagingModel>()))
                .ReturnsAsync(paginatedSpots);
            
            // Act
            var response = await controller.GetSpotsAsync(paging) as ObjectResult;
            var actualPaginatedSpots = response.Value as PaginatedModel<Spot>;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actualPaginatedSpots, typeof(PaginatedModel<Spot>));
            Assert.AreEqual(expectedSpots, actualPaginatedSpots.Data);
            Assert.IsNotNull(actualPaginatedSpots.Counts);
            Assert.AreEqual(paging, actualPaginatedSpots.Counts.Paging);
            Assert.AreEqual(expectedSpots.Count(), actualPaginatedSpots.Counts.Total);
        }
        
        [TestMethod]
        public async Task GetSpotsAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => 
                x.GetSpotsAsync(It.IsAny<PagingModel>())).Throws<Exception>();

            // Act
            var response = await controller.GetSpotsAsync(paging);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
    }
}