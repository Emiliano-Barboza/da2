using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngine.Services;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Test
{
    [TestClass]
    public class LodgmentsServiceTest
    {
        private IFixture fixture = null;
        private Mock<ILodgmentsRepository> moqRepository = null;
        private Mock<ISpotsService> moqSpotsService = null;
        private Mock<ILodgmentCalculator> moqLodgmentCalculator = null;
        private Mock<IStorageService> moqStorageService = null;
        private Mock<IReviewsService> moqReviewsService = null;
        private Mock<IBookingsService> moqBookingsService = null;
        private Lodgment expectedLodgment = null;
        private Lodgment lodgment = null;
        private Lodgment existLodgment = null;
        private int lodgmentId = 0;
        private int spotId = 0;
        private Spot existSpot = null;
        private Spot spot = null;
        private LodgmentOptionsModel lodgmentOptions = null;
        private PaginatedModel<Lodgment> expectedPaginatedLodgments = null;
        private PagingModel paging = null;
        private double totalStayPrice = 0;
        private LodgmentsService service = null;
        private FileModel fileModel = null;
        private List<string> expectedUrls = null;

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
            moqRepository = new Mock<ILodgmentsRepository>(MockBehavior.Strict);
            moqSpotsService = new Mock<ISpotsService>(MockBehavior.Strict);
            moqLodgmentCalculator = new Mock<ILodgmentCalculator>(MockBehavior.Strict);
            moqStorageService = new Mock<IStorageService>(MockBehavior.Strict);
            moqReviewsService = new Mock<IReviewsService>(MockBehavior.Strict);
            moqBookingsService = new Mock<IBookingsService>(MockBehavior.Strict);
            expectedLodgment = fixture.Create<Lodgment>();
            expectedLodgment.IsActive = true;
            expectedLodgment.IsDeleted = false;
            lodgment = fixture.Create<Lodgment>();
            existLodgment = fixture.Create<Lodgment>();
            existSpot = fixture.Create<Spot>();
            spot = fixture.Create<Spot>();
            spotId = fixture.Create<int>();
            lodgmentId = fixture.Create<int>();
            totalStayPrice = fixture.Create<double>();
            paging = fixture.Create<PagingModel>();
            fileModel = fixture.Create<FileModel>();
            expectedUrls = fixture.CreateMany<string>().ToList();
            lodgmentOptions = new LodgmentOptionsModel()
            {
                CheckIn = DateTime.Now.Ticks,
                CheckOut = DateTime.Now.Ticks + (TimeSpan.TicksPerDay * fixture.Create<int>()),
                AmountOfAdults = fixture.Create<byte>(),
                AmountOfUnderAge = fixture.Create<byte>(),
                AmountOfBabies = fixture.Create<byte>()
            };
            expectedPaginatedLodgments = fixture.Create<PaginatedModel<Lodgment>>();
            service = new LodgmentsService(moqRepository.Object, moqLodgmentCalculator.Object,
                moqStorageService.Object, moqReviewsService.Object, moqBookingsService.Object);
        }

        [TestMethod]
        public async Task AddLodgmentAsync_LodgmentNotExists_ShouldReturnTheLodgment()
        {
            // Arrange
            existLodgment = null;
            lodgment.SpotId = spot.Id;
            moqRepository.Setup(x => x.GetLodgmentByNameAsync(It.IsAny<string>())).ReturnsAsync(existLodgment);
            moqRepository.Setup(x => x.AddLodgmentAsync(It.IsAny<Lodgment>())).ReturnsAsync(expectedLodgment);

            // Act
            var actualLodgment = await service.AddLodgmentAsync(lodgment, spot);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }

        [ExpectedException(typeof(DuplicateNameException))]
        [TestMethod]
        public async Task AddLodgmentAsync_LodgmentExists_ShouldThrownException()
        {
            // Arrange
            lodgment.SpotId = spot.Id;
            moqSpotsService.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(existSpot);
            moqRepository.Setup(x => x.GetLodgmentByNameAsync(It.IsAny<string>())).ReturnsAsync(existLodgment);

            // Act
            await service.AddLodgmentAsync(lodgment, spot);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task AddLodgmentAsync_TouristSpotNotExists_ShouldThrownException()
        {
            // Arrange
            spot = null;

            // Act
            await service.AddLodgmentAsync(lodgment, spot);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task AddLodgmentAsync_LodgmentIsNull_ShouldThrownException()
        {
            // Arrange
            lodgment = null;

            // Act
            await service.AddLodgmentAsync(lodgment, spot);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task AddLodgmentAsync_SpotIdDifferent_ShouldReturnTheLodgment()
        {
            // Arrange
            existLodgment = null;
            lodgment.SpotId = spot.Id;
            spot.Id++;

            // Act
            await service.AddLodgmentAsync(lodgment, spot);
        }

        [TestMethod]
        public async Task DeleteLodgmentAsync_LodgmentNotExists_ShouldReturnTheLodgment()
        {
            // Arrange
            moqSpotsService.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(existSpot);
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);
            moqRepository.Setup(x => x.DeleteLodgmentAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);

            // Act
            var actualLodgment = await service.DeleteLodgmentAsync(lodgmentId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task DeleteLodgmentAsync_LodgmentNotExists_ShouldThrownException()
        {
            // Arrange
            expectedLodgment = null;
            moqSpotsService.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(existSpot);
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);

            // Act
            await service.DeleteLodgmentAsync(lodgmentId);
        }

        [TestMethod]
        public async Task SetLodgmentActiveStatusAsync_LodgmentExists_ShouldReturnTheLodgmentActivated()
        {
            // Arrange
            expectedLodgment.IsActive = true;
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);
            moqRepository.Setup(x => x.SetLodgmentActiveStatusAsync(It.IsAny<int>(), true))
                .ReturnsAsync(expectedLodgment);

            // Act
            var actualLodgment = await service.SetLodgmentActiveStatusAsync(lodgmentId, true);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task SetLodgmentActiveStatusAsync_LodgmentNotExists_ShouldThrownException()
        {
            // Arrange
            expectedLodgment = null;
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);

            // Act
            await service.SetLodgmentActiveStatusAsync(lodgmentId, true);
        }

        [TestMethod]
        public async Task GetLodgmentsAsync_ThereAreLodgments_ShouldReturnTheLodgments()
        {
            // Arrange
            existLodgment = null;
            moqSpotsService.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(existSpot);
            moqLodgmentCalculator.Setup(x => x.CalculateTotalStayAsync(
                It.IsAny<LodgmentOptionsModel>(), It.IsAny<Lodgment>())).ReturnsAsync(totalStayPrice);
            moqRepository.Setup(x => x.GetLodgmentsAsync(It.IsAny<PagingModel>(),
                It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(expectedPaginatedLodgments);

            // Act
            var actualPaginatedLodgments = await service.GetLodgmentsAsync(paging, lodgmentOptions, spotId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedPaginatedLodgments, actualPaginatedLodgments);
        }

        [TestMethod]
        public async Task GetLodgmentsAsync_ThereAreLodgmentsNotCalculateTotalStay_ShouldReturnTheLodgments()
        {
            // Arrange
            existLodgment = null;
            moqLodgmentCalculator.Setup(x => x.CalculateTotalStayAsync(
                It.IsAny<LodgmentOptionsModel>(), It.IsAny<Lodgment>())).ReturnsAsync(totalStayPrice);
            moqSpotsService.Setup(x => x.GetSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(existSpot);
            moqRepository.Setup(x => x.GetLodgmentsAsync(It.IsAny<PagingModel>(),
                It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(expectedPaginatedLodgments);

            // Act
            var actualPaginatedLodgments = await service.GetLodgmentsAsync(paging, lodgmentOptions, spotId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedPaginatedLodgments, actualPaginatedLodgments);
        }

        [TestMethod]
        public async Task GetLodgmentByIdAsync_LodgmentExists_ShouldReturnExpectedLodgment()
        {
            // Arrange
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(lodgmentId))
                .ReturnsAsync(expectedLodgment);

            // Act
            var actualLodgment = await service.GetLodgmentByIdAsync(lodgmentId);
            
            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [TestMethod]
        public async Task GetLodgmentByIdAsync_WithLodgmentOptions_ShouldReturnExpectedLodgment()
        {
            // Arrange
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(lodgmentId))
                .ReturnsAsync(expectedLodgment);
            moqLodgmentCalculator
                .Setup(x => x.CalculateTotalStayAsync(It.IsAny<LodgmentOptionsModel>(), It.IsAny<Lodgment>()))
                .ReturnsAsync(totalStayPrice);

            // Act
            var actualLodgment = await service.GetLodgmentByIdAsync(lodgmentId, lodgmentOptions);
            
            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedLodgment, actualLodgment);
            Assert.AreEqual(totalStayPrice, actualLodgment.TotalPrice);
        }
        
        [TestMethod]
        public async Task UploadImageAsync_LodgmentExists_ShouldReturnUploadedUrl()
        {
            // Arrange
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);
            moqRepository.Setup(x => x.AddLodgmentImageAsync(It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(expectedLodgment);
            moqStorageService.Setup(x => x.AddLodgmentImageAsync(It.IsAny<int>(), It.IsAny<FileModel>())).ReturnsAsync(expectedUrls.ToList());

            // Act
            var actualUrls = await service.UploadImageAsync(lodgmentId, fileModel);

            // Assert
            moqRepository.VerifyAll();
            moqStorageService.VerifyAll();
            CollectionAssert.AreEqual(expectedUrls, actualUrls);
        }
        
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task UploadImageAsync_LodgmentNotExists_ShouldThrownException()
        {
            // Arrange
            expectedLodgment = null;
            moqRepository.Setup(x => x.GetLodgmentByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedLodgment);

            // Act
            await service.UploadImageAsync(lodgmentId, fileModel);
        }
    }
}