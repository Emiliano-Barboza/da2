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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Test
{
    [TestClass]
    public class RegionsRepositoryTest
    {
        private IFixture fixture = null;
        private DbContextOptionsBuilder<NaturalUruguayContext> contextBuilderOptions = null;
        private NaturalUruguayContext context = null;
        private IEnumerable<Region> expectedRegions = null;
        private Region expectedRegion = null;
        private int regionId = 0;
        private PagingModel paging = null;
        private IRegionsRepository repository = null;
        private const string DataBaseName = "NaturalUruguayDB";
        private const string OrderByIdKey = "Id";

        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<Region>(c => c
                .Without(r => r.Spots));
            expectedRegions = fixture.CreateMany<Region>();
            expectedRegion = fixture.Create<Region>();
            regionId = fixture.Create<int>();
            paging = fixture.Create<PagingModel>();
            contextBuilderOptions = new DbContextOptionsBuilder<NaturalUruguayContext>();
            contextBuilderOptions.UseInMemoryDatabase(databaseName: DataBaseName);
            context = new NaturalUruguayContext(contextBuilderOptions.Options);
            context.Database.EnsureDeleted();
            
            repository = new RegionsRepository(context);
        }

        [TestMethod]
        public async Task GetSpotsAsync_NoRestrictions_ShouldReturnTheRegions()
        {
            // Arrange
            paging = new PagingModel();
            paging.Order = OrderByIdKey;
            await context.AddRangeAsync(expectedRegions);
            await context.SaveChangesAsync();
            
            // Act
            var actualPaginatedRegions = await repository.GetSpotsAsync(paging);

            // Assert
            Assert.AreEqual(expectedRegions.Count(), actualPaginatedRegions.Data.Count());
        }
        
        [TestMethod]
        public async Task GetSpotsAsync_FilterByName_ShouldReturnTheRegions()
        {
            // Arrange
            paging = new PagingModel();
            paging.Order = OrderByIdKey;
            paging.FilterBy = expectedRegions.First().Name;
            var expectedCount = 1;
            await context.AddRangeAsync(expectedRegions);
            await context.SaveChangesAsync();
            
            // Act
            var actualPaginatedRegions = await repository.GetSpotsAsync(paging);

            // Assert
            Assert.AreEqual(expectedCount, actualPaginatedRegions.Data.Count());
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetSpotsAsync_PagingModelIsNull_ShouldThrownException()
        {
            // Arrange
            paging = null;
            
            // Act
            await repository.GetSpotsAsync(paging);
        }
        
        [TestMethod]
        public async Task GetRegionByIdAsync_RegionExists_ShouldReturnTheRegion()
        {
            // Arrange 
            expectedRegion.Id = regionId;
            context.Add(expectedRegion);
            context.SaveChanges();
            
            //Act
            var actualRegion = await repository.GetRegionByIdAsync(regionId);

            //Assert
            Assert.AreEqual(expectedRegion, actualRegion);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetRegionByIdAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetRegionByIdAsync(regionId);
        }
    }
}