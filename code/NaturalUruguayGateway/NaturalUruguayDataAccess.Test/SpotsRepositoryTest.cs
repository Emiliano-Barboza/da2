using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.DataAccess.Context;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Test
{
    [TestClass]
    public class SpotsRepositoryTest
    {
        private IFixture fixture = null;
        private DbContextOptionsBuilder<NaturalUruguayContext> contextBuilderOptions = null;
        private NaturalUruguayContext context = null;
        private IEnumerable<Spot> expectedSpots = null;
        private Spot expectedSpot = null;
        private Category expectedCategory = null;
        private int spotId = 0;
        private int regionId = 0;
        private string spotName = "";
        private ISpotsRepository repository = null;
        private PagingModel paging = null;
        private const string DataBaseName = "NaturalUruguayDB";

        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<Spot>(c => c
                .Without(x => x.Region));
            fixture.Customize<Lodgment>(c => c
                .Without(x => x.Spot)
                .Without(l => l.Reviews)
                .Without(l => l.Bookings));
            fixture.Customize<CategorySpot>(c => c
                .Without(x => x.Category)
                .Without(s => s.Spot));
            expectedSpots = fixture.CreateMany<Spot>();
            expectedSpot = fixture.Create<Spot>();
            expectedCategory = fixture.Create<Category>();
            spotId = fixture.Create<int>();
            regionId = fixture.Create<int>();
            spotName = fixture.Create<string>();
            paging = fixture.Create<PagingModel>();
            contextBuilderOptions = new DbContextOptionsBuilder<NaturalUruguayContext>();
            contextBuilderOptions.UseInMemoryDatabase(databaseName: DataBaseName);
            context = new NaturalUruguayContext(contextBuilderOptions.Options);
            context.Database.EnsureDeleted();
            
            repository = new SpotsRepository(context);
        }

        [TestMethod]
        public async Task GetSpotByIdAsync_SpotExists_ShouldReturnTheSpot()
        {
            // Arrange 
            expectedSpot.Region = fixture.Create<Region>();
            spotId = expectedSpot.Id;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualSpot = await repository.GetSpotByIdAsync(spotId);

            //Assert
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [TestMethod]
        public async Task GetSpotByIdAsync_SpotNotExists_ShouldReturnNull()
        {
            // Arrange 
            spotId = 10;
            expectedSpot.Id = 1;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualSpot = await repository.GetSpotByIdAsync(spotId);

            //Assert
            Assert.IsNull(actualSpot);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetSpotByIdAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetSpotByIdAsync(spotId);
        }
        
        [TestMethod]
        public async Task GetSpotByNameAsync_SpotExists_ShouldReturnTheSpot()
        {
            // Arrange 
            spotName = expectedSpot.Name;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualSpot = await repository.GetSpotByNameAsync(spotName);

            //Assert
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [TestMethod]
        public async Task GetSpotByNameAsync_SpotNotExists_ShouldReturnNull()
        {
            // Arrange 
            spotName = "Some other place";
            expectedSpot.Name = "Some place";
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualSpot = await repository.GetSpotByNameAsync(spotName);

            //Assert
            Assert.IsNull(actualSpot);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetSpotByNameAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetSpotByNameAsync(spotName);
        }
        
        [TestMethod]
        public async Task AddSpotAsync_SpotNotExists_ShouldReturnTheSpot()
        {
            // Arrange 
            
            //Act
            var actualSpot = await repository.AddSpotAsync(expectedSpot);

            //Assert
            Assert.AreEqual(expectedSpot, actualSpot);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task AddSpotAsync_SpotExists_ShouldThrownException()
        {
            // Arrange 
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            await repository.AddSpotAsync(expectedSpot);
        }
        
        [TestMethod]
        public async Task GetSpotsByRegionIdAsync_RegionAndSpotsExist_ShouldReturnThePaginatedSpots()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            regionId = expectedSpot.RegionId;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualPaginatedSpots = await repository.GetSpotsByRegionIdAsync(regionId, paging);

            //Assert
            Assert.IsTrue(actualPaginatedSpots.Data.Count() == 1);
            Assert.IsTrue(actualPaginatedSpots.Counts.Total == 1);
            Assert.AreEqual(expectedSpot, actualPaginatedSpots.Data.First());
        }
        
        [TestMethod]
        public async Task GetSpotsByRegionIdAsync_RegionAndSpotsExistFilterByCategory_ShouldReturnThePaginatedSpots()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "CategorySpots";
            
            paging.FilterBy = expectedSpot.CategorySpots.Select(x => x.CategoryId.ToString()).Join();
            regionId = expectedSpot.RegionId;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualPaginatedSpots = await repository.GetSpotsByRegionIdAsync(regionId, paging);

            //Assert
            Assert.IsTrue(actualPaginatedSpots.Data.Count() == 1);
            Assert.IsTrue(actualPaginatedSpots.Counts.Total == 1);
            Assert.AreEqual(expectedSpot, actualPaginatedSpots.Data.First());
        }
        
        [TestMethod]
        public async Task GetSpotsByRegionIdAsync_RegionAndSpotsExistFilterBy_ShouldReturnThePaginatedSpots()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            
            paging.FilterBy = expectedSpot.Name;
            regionId = expectedSpot.RegionId;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualPaginatedSpots = await repository.GetSpotsByRegionIdAsync(regionId, paging);

            //Assert
            Assert.IsTrue(actualPaginatedSpots.Data.Count() == 1);
            Assert.IsTrue(actualPaginatedSpots.Counts.Total == 1);
            Assert.AreEqual(expectedSpot, actualPaginatedSpots.Data.First());
        }
        
        [TestMethod]
        public async Task GetSpotsByRegionIdAsync_RegionAndSpotsExistFilterByWrongCategory_ShouldReturnThePaginatedSpots()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "CategorySpots";
            
            paging.FilterBy = "1000000000000";
            regionId = expectedSpot.RegionId;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualPaginatedSpots = await repository.GetSpotsByRegionIdAsync(regionId, paging);

            //Assert
            Assert.IsTrue(!actualPaginatedSpots.Data.Any());
            Assert.IsTrue(actualPaginatedSpots.Counts.Total == 0);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetSpotsByRegionIdAsync_PaginModelNull_ShouldThrownException()
        {
            // Arrange 
            paging = null;
            regionId = expectedSpot.RegionId;
            context.Add(expectedSpot);
            context.SaveChanges();
            
            //Act
            var actualPaginatedSpots = await repository.GetSpotsByRegionIdAsync(regionId, paging);

            //Assert
            Assert.IsTrue(!actualPaginatedSpots.Data.Any());
            Assert.IsTrue(actualPaginatedSpots.Counts.Total == 0);
        }
        
        [TestMethod]
        public async Task GetSpotDefaultCategoryAsync_HasCategories_ShouldReturnTheCategory()
        {
            // Arrange 
            context.Add(expectedCategory);
            context.SaveChanges();
            
            //Act
            var actualCategory = await repository.GetSpotDefaultCategoryAsync();

            //Assert
            Assert.AreEqual(expectedCategory, actualCategory);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetSpotDefaultCategoryAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetSpotDefaultCategoryAsync();
        }
    }
}