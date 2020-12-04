using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngine.Services;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Test
{
    [TestClass]
    public class RegionsServiceTest
    {
        private IFixture fixture = null;
        private IEnumerable<Region> expectedRegions = null;
        private Mock<IRegionsRepository> moqRepository = null;
        private Mock<ISpotsService> moqSpotsService = null;
        private Spot expectedSpot = null;
        private Spot spot = null;
        private Region expectedRegion = null;
        private int regionId = 0;
        private PaginatedModel<Spot> expectedPaginatedSpots = null;
        private PaginatedModel<Region> expectedPaginatedRegions = null;
        private PagingModel paging = null;
        private RegionsService service = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<Spot>(c => c
                .Without(x => x.Region));
            fixture.Customize<Lodgment>(c => c
                .Without(x => x.Spot)
                .Without(y => y.Bookings)
                .Without(y => y.Reviews));
            fixture.Customize<CategorySpot>(c => c
                .Without(x => x.Category)
                .Without(x => x.Spot));
            moqRepository = new Mock<IRegionsRepository>(MockBehavior.Strict);
            moqSpotsService = new Mock<ISpotsService>(MockBehavior.Strict);
            expectedSpot = fixture.Create<Spot>();
            expectedRegions = fixture.CreateMany<Region>();
            spot = fixture.Create<Spot>();
            expectedRegion = fixture.Create<Region>();
            regionId = fixture.Create<int>();
            expectedPaginatedSpots = fixture.Create<PaginatedModel<Spot>>();
            expectedPaginatedRegions = fixture.Create<PaginatedModel<Region>>();
            paging = fixture.Create<PagingModel>();
            service = new RegionsService(moqRepository.Object, moqSpotsService.Object);
        }
        
        [TestMethod]
        public async Task GetRegionsAsync_NoRestrictions_ShouldReturnTheRegions()
        {
            //Arrange
            expectedPaginatedRegions.Data = expectedRegions;
            moqRepository.Setup(x => x.GetSpotsAsync(It.IsAny<PagingModel>())).ReturnsAsync(expectedPaginatedRegions);
            
            //Act
            var actualPaginatedRegions = await service.GetRegionsAsync(paging);

            //Assert
            Assert.AreEqual(expectedRegions, actualPaginatedRegions.Data);
        }
        
        [TestMethod]
        public async Task GetRegionByIdAsync_CallingTheRepository_ShouldReturnTheRegion()
        {
            // Arrange
            moqRepository.Setup(x => x.GetRegionByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedRegion);

            // Act
            var actualRegion = await service.GetRegionByIdAsync(regionId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedRegion, actualRegion);
        }
        
        [TestMethod]
        public async Task AddSpotToRegionAsync_RegionExists_ShouldReturnTheSpot()
        {
            // Arrange
            moqRepository.Setup(x => x.GetRegionByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedRegion);
            moqSpotsService.Setup(x => x.AddSpotAsync(It.IsAny<Spot>(), It.IsAny<Region>())).ReturnsAsync(expectedSpot);

            // Act
            var actualSpot = await service.AddSpotToRegionAsync(spot);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task AddSpotToRegionAsync_RegionNotExists_ShouldThrownException()
        {
            // Arrange
            expectedRegion = null;
            moqRepository.Setup(x => x.GetRegionByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedRegion);

            // Act
            await service.AddSpotToRegionAsync(spot);
        }
        
        [TestMethod]
        public async Task GetSpotsByRegionAsync_RegionExists_ShouldReturnThePaginatedSpots()
        {
            // Arrange
            moqRepository.Setup(x => x.GetRegionByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedRegion);
            moqSpotsService.Setup(x => x.GetSpotsByRegionIdAsync(It.IsAny<int>(), It.IsAny<PagingModel>())).ReturnsAsync(expectedPaginatedSpots);

            // Act
            var actualPaginatedSpots = await service.GetSpotsByRegionAsync(regionId, paging);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedPaginatedSpots, actualPaginatedSpots);
        }
        
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task GetSpotsByRegionAsync_RegionNotExists_ShouldThrownException()
        {
            // Arrange
            expectedRegion = null;
            moqRepository.Setup(x => x.GetRegionByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedRegion);

            // Act
            var actualPaginatedSpots = await service.GetSpotsByRegionAsync(regionId, paging);
        }
    }
}