using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.HelperInterface.Services;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngine.Services;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Test
{
    [TestClass]
    public class SpotsServiceTest
    {
        private IFixture fixture = null;
        private Mock<ISpotsRepository> moqRepository = null;
        private Mock<ILodgmentsService> moqLodgmentsService = null;
        private Mock<IStorageService> moqStorageService = null;
        private Mock<IReportsService> moqReportsService = null;
        private Spot expectedSpot = null;
        private Spot spot = null;
        private int spotId = 0;
        private Spot existSpot = null;
        private Category existCategory = null;
        private CategorySpot expectedCategorySpot = null;
        private Region existRegion = null;
        private Region region = null;
        private Lodgment expectedLodgment = null;
        private Lodgment lodgment = null;
        private int lodgmentId = 0;
        private int regionId = 0;
        private PaginatedModel<Lodgment> expectedPaginatedLodgments = null;
        private PaginatedModel<Spot> expectedPaginatedSpots = null;
        private IEnumerable<Lodgment> expectedLodgments = null;
        private IEnumerable<Spot> expectedSpots = null;
        private PagingModel paging = null;
        private LodgmentOptionsModel lodgmentOptions = null;
        private SpotsService service = null;
        private PaginatedModel<Spot> paginatedSpots = null;

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
            moqRepository = new Mock<ISpotsRepository>(MockBehavior.Strict);
            moqLodgmentsService = new Mock<ILodgmentsService>(MockBehavior.Strict);
            moqStorageService = new Mock<IStorageService>(MockBehavior.Strict);
            moqReportsService = new Mock<IReportsService>(MockBehavior.Strict);
            expectedSpot = fixture.Create<Spot>();
            spot = fixture.Create<Spot>();
            spotId = fixture.Create<int>();
            existSpot = fixture.Create<Spot>();
            existCategory = fixture.Create<Category>();
            expectedCategorySpot = fixture.Create<CategorySpot>();
            existRegion = fixture.Create<Region>();
            region = fixture.Create<Region>();
            expectedLodgment = fixture.Create<Lodgment>();
            lodgment = fixture.Create<Lodgment>();
            expectedLodgments = fixture.CreateMany<Lodgment>();
            expectedSpots = fixture.CreateMany<Spot>();
            lodgmentId = fixture.Create<int>();
            paging = fixture.Create<PagingModel>();
            lodgmentOptions = new LodgmentOptionsModel(){
                CheckIn = DateTime.Now.Ticks,
                CheckOut = DateTime.Now.Ticks + (TimeSpan.TicksPerDay * fixture.Create<int>()),
                AmountOfAdults = fixture.Create<byte>(),
                AmountOfUnderAge = fixture.Create<byte>(),
                AmountOfBabies = fixture.Create<byte>()
            };
            expectedPaginatedLodgments = fixture.Create<PaginatedModel<Lodgment>>();
            expectedPaginatedSpots = fixture.Create<PaginatedModel<Spot>>();
            service = new SpotsService(moqRepository.Object, moqLodgmentsService.Object,
                moqStorageService.Object, moqReportsService.Object);
            paginatedSpots = fixture.Create<PaginatedModel<Spot>>();
        }
        
        [TestMethod]
        public async Task AddSpotAsync_RegionExistsAndSpotNotExists_ShouldReturnTheSpot()
        {
            // Arrange
            existSpot = null;
            spot.RegionId = region.Id;
            moqRepository.Setup(x => x.GetSpotByNameAsync(It.IsAny<string>())).ReturnsAsync(existSpot);
            moqRepository.Setup(x => x.AddSpotAsync(It.IsAny<Spot>())).ReturnsAsync(expectedSpot);

            // Act
            var actualSpot = await service.AddSpotAsync(spot, region);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [TestMethod]
        public async Task AddSpotAsync_RegionExistsAndSpotNotExistsWithoutCategory_ShouldReturnTheSpotWithCategory()
        {
            // Arrange
            existSpot = null;
            spot.RegionId = region.Id;
            spot.CategorySpots = null;
            expectedCategorySpot.CategoryId = existCategory.Id;
            expectedSpot.CategorySpots.Clear();
            expectedSpot.CategorySpots.Add(expectedCategorySpot);
            moqRepository.Setup(x => x.GetSpotByNameAsync(It.IsAny<string>())).ReturnsAsync(existSpot);
            moqRepository.Setup(x => x.AddSpotAsync(It.IsAny<Spot>())).ReturnsAsync(expectedSpot);
            moqRepository.Setup(x => x.GetSpotDefaultCategoryAsync()).ReturnsAsync(existCategory);

            // Act
            var actualSpot = await service.AddSpotAsync(spot, region);

            // Assert
            moqRepository.VerifyAll();
            var actualCategorySpots = actualSpot.CategorySpots; 
            var actualCategory = actualCategorySpots.First(); 
            var expectedCategorySpots = expectedSpot.CategorySpots; 
            Assert.AreEqual(expectedSpot, actualSpot);
            Assert.IsNotNull(expectedSpot.CategorySpots);
            Assert.AreEqual(expectedCategorySpots.Count, actualCategorySpots.Count);
            Assert.AreEqual(expectedCategorySpot.CategoryId, actualCategory.CategoryId);
        }

        
        [ExpectedException(typeof(DuplicateNameException))]
        [TestMethod]
        public async Task AddSpotAsync_RegionExistsAndSpotExists_ShouldThrownException()
        {
            // Arrange
            spot.RegionId = region.Id;
            moqRepository.Setup(x => x.GetSpotByNameAsync(It.IsAny<string>())).ReturnsAsync(existSpot);

            // Act
            await service.AddSpotAsync(spot, region);
        }
        
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task AddSpotAsync_RegionNotTheSameInSpot_ShouldThrownException()
        {
            // Arrange
            spot.RegionId = 0;
            region.Id = 1;

            // Act
            await service.AddSpotAsync(spot, region);
        }
        
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task AddSpotAsync_SpotModelIsNull_ShouldThrownException()
        {
            // Arrange
            spot = null;

            // Act
            await service.AddSpotAsync(spot, region);
        }
        
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task AddSpotAsync_RegionModelIsNull_ShouldThrownException()
        {
            // Arrange
            region = null;

            // Act
            await service.AddSpotAsync(spot, region);
        }
        
        [TestMethod]
        public async Task GetSpotByIdAsync_CallingTheSpotsRepository_ShouldReturnTheSpot()
        {
            // Arrange
            moqRepository.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSpot);

            // Act
            var actualSpot = await service.GetSpotByIdAsync(spotId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [TestMethod]
        public async Task AddLodgmentToSpotAsync_SpotExists_ShouldReturnTheLodgment()
        {
            // Arrange
            moqRepository.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSpot);
            moqLodgmentsService.Setup(x => x.AddLodgmentAsync(It.IsAny<Lodgment>(), It.IsAny<Spot>())).ReturnsAsync(expectedLodgment);

            // Act
            var actualLodgment = await service.AddLodgmentToSpotAsync(lodgment);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task AddLodgmentToSpotAsync_SpotNotExists_ShouldThrownException()
        {
            // Arrange
            expectedSpot = null;
            moqRepository.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSpot);
            
            // Act
            await service.AddLodgmentToSpotAsync(lodgment);
        }
        
        [TestMethod]
        public async Task DeleteLodgmentInSpotAsync_CallingTheLodgmentsService_ShouldReturnTheLodgment()
        {
            // Arrange
            moqRepository.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSpot);
            moqLodgmentsService.Setup(x => x.DeleteLodgmentAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);

            // Act
            var actualLodgment = await service.DeleteLodgmentInSpotAsync(spotId, lodgmentId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task DeleteLodgmentInSpotAsync_SpotNotExists_ShouldThrownException()
        {
            // Arrange
            expectedSpot = null;
            moqRepository.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSpot);

            // Act
            await service.DeleteLodgmentInSpotAsync(spotId, lodgmentId);
        }
        
        [TestMethod]
        public async Task GetLodgmentsInSpotAsync_CallingTheLodgmentsService_ShouldReturnThePaginatedLodgments()
        {
            // Arrange
            expectedPaginatedLodgments.Data = expectedLodgments;
            moqLodgmentsService.Setup(x => x.GetLodgmentsAsync(It.IsAny<PagingModel>(),
                It.IsAny<LodgmentOptionsModel>(), It.IsAny<int>(),false)).ReturnsAsync(expectedPaginatedLodgments);

            // Act
            var actualPaginatedLodgments = await service.GetLodgmentsInSpotAsync(spotId, lodgmentOptions, paging);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedPaginatedLodgments, actualPaginatedLodgments);
        }
        
        [TestMethod]
        public async Task GetSpotsByRegionIdAsync_CallingTheRepository_ShouldReturnThePaginatedSpots()
        {
            // Arrange
            expectedPaginatedSpots.Data = expectedSpots;
            moqRepository.Setup(x => 
                x.GetSpotsByRegionIdAsync(It.IsAny<int>(), It.IsAny<PagingModel>())).ReturnsAsync(expectedPaginatedSpots);
            
            // Act
            var actualPaginatedSpots = await service.GetSpotsByRegionIdAsync(regionId, paging);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedPaginatedSpots, actualPaginatedSpots);
        }
        
        [TestMethod]
        public async Task GetSpotsAsync_NoRestrictions_ShouldReturnTheSpots()
        {
            // Arrange
            moqRepository.Setup(x => x.GetSpotsAsync(It.IsAny<PagingModel>())).ReturnsAsync(expectedPaginatedSpots);

            // Act
            var actualPaginatedSpots = await service.GetSpotsAsync(paging);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedPaginatedSpots, actualPaginatedSpots);
        }
    }
}