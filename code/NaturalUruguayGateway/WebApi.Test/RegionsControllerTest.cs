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
    public class RegionsControllerTest : BaseControllerTest
    {
        private Mock<IRegionsService> moqService = null;
        private RegionsController controller = null;
        private IEnumerable<Region> expectedRegions = null;
        private Spot expectedSpot = null;
        private Spot spot = null;
        private IEnumerable<Spot> expectedSpots = null;
        private PaginatedModel<Spot> paginatedSpots = null;
        private PaginatedModel<Region> paginatedRegions = null;
        private PagingModel paging = null;
        private int regionId = 0;
        
        [TestInitialize]
        public void BeforeEach()
        {
            InitializeComponents();
            fixture.Customize<Spot>(c => c
                .Without(x => x.Region));
            fixture.Customize<Lodgment>(c => c
                .Without(x => x.Spot)
                .Without(y => y.Reviews)
                .Without(y => y.Bookings));
            fixture.Customize<CategorySpot>(c => c
                .Without(x => x.Category)
                .Without(x => x.Spot));
            moqService = new Mock<IRegionsService>(MockBehavior.Strict);
            controller = new RegionsController(moqService.Object);
            controller.ControllerContext = controllerContext;
            expectedRegions = fixture.CreateMany<Region>();
            expectedSpot = fixture.Create<Spot>();
            expectedSpots = fixture.CreateMany<Spot>();
            paginatedSpots = fixture.Create<PaginatedModel<Spot>>();
            paginatedRegions = fixture.Create<PaginatedModel<Region>>();
            paging = fixture.Create<PagingModel>();
            spot = fixture.Create<Spot>();
            regionId = fixture.Create<int>();
        }
        
        [TestMethod]
        public async Task GetRegionsAsync_NoRestrictions_Success()
        {
            //Arrange
            paginatedRegions.Data = expectedRegions;
            paginatedRegions.Counts = new PaginatedCountModel(paging, expectedRegions.Count());
            moqService.Setup(x => x.GetRegionsAsync(It.IsAny<PagingModel>()))
              .ReturnsAsync(paginatedRegions);
            
            //Act
            var response = await controller.GetRegionsAsync(paging) as ObjectResult;
            var actualPaginatedRegions = response?.Value as PaginatedModel<Region>;

            //Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actualPaginatedRegions, typeof(PaginatedModel<Region>));
            Assert.AreEqual(expectedRegions, actualPaginatedRegions.Data);
            Assert.IsNotNull(actualPaginatedRegions.Counts);
            Assert.AreEqual(paging, actualPaginatedRegions.Counts.Paging);
            Assert.AreEqual(expectedRegions.Count(), actualPaginatedRegions.Counts.Total);
        }
        
        [TestMethod]
        public async Task AddSpotToRegionAsync_Success()
        {
            // Arrange
            moqService.Setup(x => x.AddSpotToRegionAsync(It.IsAny<Spot>())).ReturnsAsync(expectedSpot);
            
            // Act
            var response = await controller.AddSpotToRegionAsync(regionId, spot) as ObjectResult;
            var actualSpot = response.Value;
            
            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [TestMethod]
        public async Task AddSpotToRegionAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.AddSpotToRegionAsync(It.IsAny<Spot>())).Throws<Exception>();
            
            // Act
            var response = await controller.AddSpotToRegionAsync(regionId, spot);

            // Assert
            moqService.Verify();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetSpotsByRegionAsync_CallingTheService_Success()
        {
            // Arrange
            paginatedSpots.Data = expectedSpots;
            paginatedSpots.Counts = new PaginatedCountModel(paging, expectedSpots.Count());
            moqService.Setup(x => 
                    x.GetSpotsByRegionAsync(It.IsAny<int>(), It.IsAny<PagingModel>())).ReturnsAsync(paginatedSpots);
            
            // Act
            var response = await controller.GetSpotsByRegionAsync(regionId, paging) as ObjectResult;
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
        public async Task GetSpotsByRegionAsync_CallingTheService_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => 
                x.GetSpotsByRegionAsync(It.IsAny<int>(), It.IsAny<PagingModel>())).Throws<Exception>();

            // Act
            var response = await controller.GetSpotsByRegionAsync(regionId, paging);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
    }
}