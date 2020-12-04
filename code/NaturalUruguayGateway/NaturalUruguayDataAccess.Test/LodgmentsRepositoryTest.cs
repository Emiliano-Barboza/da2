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
    public class LodgmentsRepositoryTest
    {
        private IFixture fixture = null;
        private DbContextOptionsBuilder<NaturalUruguayContext> contextBuilderOptions = null;
        private NaturalUruguayContext context = null;
        private IEnumerable<Lodgment> expectedLodgments = null;
        private Lodgment expectedLodgment = null;
        private int lodgmentId = 0;
        private string lodgmentName = null;
        private List<string> urls = null;
        private PagingModel paging = null;
        private ILodgmentsRepository repository = null;
        private const string DataBaseName = "NaturalUruguayDB";
        private const string OrderByIdKey = "Id";

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
                .Without(x => x.Spot));
            expectedLodgments = fixture.CreateMany<Lodgment>();
            expectedLodgment = fixture.Create<Lodgment>();
            lodgmentId = fixture.Create<int>();
            lodgmentName = fixture.Create<string>();
            urls = fixture.CreateMany<string>().ToList();
            paging = fixture.Create<PagingModel>();
            contextBuilderOptions = new DbContextOptionsBuilder<NaturalUruguayContext>();
            contextBuilderOptions.UseInMemoryDatabase(databaseName: DataBaseName);
            context = new NaturalUruguayContext(contextBuilderOptions.Options);
            context.Database.EnsureDeleted();
            
            repository = new LodgmentsRepository(context);
        }

        [TestMethod]
        public async Task AddLodgmentAsync_Norestrictions_ShouldReturnTheLodgment()
        {
            // Arrange 
            
            //Act
            var actualLodgment = await repository.AddLodgmentAsync(expectedLodgment);

            //Assert
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task AddLodgmentAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.AddLodgmentAsync(expectedLodgment);
        }
        
        [TestMethod]
        public async Task GetLodgmentByNameAsync_LodgmentExists_ShouldReturnTheLodgment()
        {
            // Arrange 
            expectedLodgment.Name = lodgmentName;
            expectedLodgment.IsActive = true;
            expectedLodgment.IsDeleted = false;
            context.Add(expectedLodgment);
            context.SaveChanges();
            
            //Act
            var actualLodgment = await repository.GetLodgmentByNameAsync(lodgmentName);

            //Assert
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetLodgmentByNameAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetLodgmentByNameAsync(lodgmentName);
        }
        
        [TestMethod]
        public async Task GetLodgmentByIdAsync_LodgmentExists_ShouldReturnTheLodgment()
        {
            // Arrange 
            expectedLodgment.Id = lodgmentId;
            expectedLodgment.IsActive = true;
            expectedLodgment.IsDeleted = false;
            expectedLodgment.Spot = fixture.Create<Spot>();
            expectedLodgment.Spot.Region = fixture.Create<Region>();
            context.Add(expectedLodgment);
            context.SaveChanges();
            
            //Act
            var actualLodgment = await repository.GetLodgmentByIdAsync(lodgmentId);

            //Assert
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetLodgmentByIdAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetLodgmentByIdAsync(lodgmentId);
        }
        
        [TestMethod]
        public async Task DeleteLodgmentAsync_LodgmentExists_ShouldReturnTheLodgment()
        {
            // Arrange 
            expectedLodgment.Id = lodgmentId;
            context.Add(expectedLodgment);
            context.SaveChanges();
            
            //Act
            var actualLodgment = await repository.DeleteLodgmentAsync(lodgmentId);

            //Assert
            Assert.AreEqual(expectedLodgment, actualLodgment);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task DeleteLodgmentAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.DeleteLodgmentAsync(lodgmentId);
        }
        
        [TestMethod]
        public async Task SetLodgmentActiveStatusAsync_LodgmentExists_ShouldReturnTheLodgmentActive()
        {
            // Arrange 
            var isActive = true;
            expectedLodgment.IsActive = false;
            expectedLodgment.Id = lodgmentId;
            context.Add(expectedLodgment);
            context.SaveChanges();
            
            //Act
            var actualLodgment = await repository.SetLodgmentActiveStatusAsync(lodgmentId, isActive);

            //Assert
            Assert.AreEqual(expectedLodgment.Id, actualLodgment.Id);
            Assert.AreEqual(expectedLodgment.IsActive, actualLodgment.IsActive);
        }
        
        [TestMethod]
        public async Task SetLodgmentActiveStatusAsync_LodgmentExists_ShouldReturnTheLodgmentNotActive()
        {
            // Arrange 
            var isActive = false;
            expectedLodgment.IsActive = true;
            expectedLodgment.Id = lodgmentId;
            context.Add(expectedLodgment);
            context.SaveChanges();
            
            //Act
            var actualLodgment = await repository.SetLodgmentActiveStatusAsync(lodgmentId, isActive);

            //Assert
            Assert.AreEqual(expectedLodgment.Id, actualLodgment.Id);
            Assert.AreEqual(expectedLodgment.IsActive, actualLodgment.IsActive);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task SetLodgmentActiveStatusAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            var isActive = true;
            await context.DisposeAsync();
            
            //Act
            await repository.SetLodgmentActiveStatusAsync(lodgmentId,isActive);
        }
        
        [TestMethod]
        public async Task GetLodgmentsAsync_ThereAreLodgments_ShouldReturnThePaginatedLodgments()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            expectedLodgment.IsActive = true;
            expectedLodgment.IsDeleted = false;
            context.Add(expectedLodgment);
            await context.SaveChangesAsync();
            
            //Act
            var actualPaginatedLodgments = await repository.GetLodgmentsAsync(paging);

            //Assert
            Assert.AreEqual(expectedLodgment, actualPaginatedLodgments.Data.First());
        }
        
        [TestMethod]
        public async Task GetLodgmentsAsync_LodgmentExistsFilterBy_ShouldReturnThePaginatedLodgments()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            paging.FilterBy = expectedLodgment.Name;
            expectedLodgment.IsActive = true;
            expectedLodgment.IsDeleted = false;
            context.Add(expectedLodgment);
            context.SaveChanges();
            
            //Act
            var actualPaginatedUsers = await repository.GetLodgmentsAsync(paging);

            //Assert
            Assert.IsTrue(actualPaginatedUsers.Data.Count() == 1);
            Assert.IsTrue(actualPaginatedUsers.Counts.Total == 1);
            Assert.AreEqual(expectedLodgment, actualPaginatedUsers.Data.First());
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetLodgmentsAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange 
            await context.DisposeAsync();
            
            //Act
            await repository.GetLodgmentsAsync(paging);
        }
        
        [TestMethod]
        public async Task AddLodgmentImageAsync_LodgmentExists_ShouldReturnTheLodgment()
        {
            // Arrange
            expectedLodgment.Id = lodgmentId;
            expectedLodgment.IsActive = true;
            expectedLodgment.IsDeleted = false;
            expectedLodgment.Images.Clear();
            context.Add(expectedLodgment);
            context.SaveChanges();
            
            //Act
            var actualLodgment = await repository.AddLodgmentImageAsync(lodgmentId, urls);

            //Assert
            Assert.IsNotNull(actualLodgment);
            Assert.AreEqual(expectedLodgment.Id, actualLodgment.Id);
            CollectionAssert.AreEqual(urls, actualLodgment.Images.ToList());
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task AddLodgmentImageAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange 
            await context.DisposeAsync();
            
            //Act
            await repository.AddLodgmentImageAsync(lodgmentId, urls);
        }
    }
}