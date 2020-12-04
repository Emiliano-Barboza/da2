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
    public class UsersRepositoryTest
    {
        private IFixture fixture = null;
        private DbContextOptionsBuilder<NaturalUruguayContext> contextBuilderOptions = null;
        private NaturalUruguayContext context = null;
        private IEnumerable<User> expectedUsers = null;
        private User user = null;
        private User expectedUser = null;
        private UserRole expectedUserRole = null;
        private int userId = 0;
        private PagingModel paging = null;
        private IUsersRepository repository = null;
        private const string DataBaseName = "NaturalUruguayDB";
        private const string OrderByIdKey = "Id";

        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<Region>(c => c
                .Without(r => r.Spots));
            expectedUsers = fixture.CreateMany<User>();
            expectedUser = fixture.Create<User>();
            expectedUserRole = fixture.Create<UserRole>();
            user = fixture.Create<User>();
            userId = fixture.Create<int>();
            paging = fixture.Create<PagingModel>();
            contextBuilderOptions = new DbContextOptionsBuilder<NaturalUruguayContext>();
            contextBuilderOptions.UseInMemoryDatabase(databaseName: DataBaseName);
            context = new NaturalUruguayContext(contextBuilderOptions.Options);
            context.Database.EnsureDeleted();
            
            repository = new UsersRepository(context);
        }

        [TestMethod]
        public async Task AddUserAsync_NoRestrictions_ShouldReturnTheUser()
        {
            // Arrange
            expectedUser = user;
            
            // Act
            var actualUser = await repository.AddUserAsync(user);

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task AddUserAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.AddUserAsync(user);
        }
        
        [TestMethod]
        public async Task DeleteUserByIdAsync_UserExists_ShouldReturnTheUser()
        {
            // Arrange
            context.Add(expectedUser);
            context.SaveChanges();
            
            // Act
            var actualUser = await repository.DeleteUserByIdAsync(expectedUser.Id);

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task DeleteUserByIdAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.DeleteUserByIdAsync(expectedUser.Id);
        }
        
        [TestMethod]
        public async Task UpdateUserAsync_UserExists_ShouldReturnTheUser()
        {
            // Arrange
            user.Id = expectedUser.Id;
            context.Add(user);
            context.SaveChanges();
            
            // Act
            var actualUser = await repository.UpdateUserAsync(expectedUser);

            // Assert
            Assert.AreEqual(expectedUser.Email, actualUser.Email);
            Assert.AreEqual(expectedUser.Name, actualUser.Name);
            Assert.AreEqual(expectedUser.Password, actualUser.Password);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task UpdateUserAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.UpdateUserAsync(expectedUser);
        }
        
        [TestMethod]
        public async Task GetUserByEmailAsync_UserExists_ShouldReturnTheUser()
        {
            // Arrange
            context.Add(expectedUser);
            context.SaveChanges();
            
            // Act
            var actualUser = await repository.GetUserByEmailAsync(expectedUser.Email);

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetUserByEmailAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetUserByEmailAsync(expectedUser.Email);
        }
        
        [TestMethod]
        public async Task GetUserByIdAsync_UserExists_ShouldReturnTheUser()
        {
            // Arrange
            context.Add(expectedUser);
            context.SaveChanges();
            
            // Act
            var actualUser = await repository.GetUserByIdAsync(expectedUser.Id);

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetUserByIdAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetUserByIdAsync(expectedUser.Id);
        }
        
        [TestMethod]
        public async Task GetUserRoleByNameAsync_UserExists_ShouldReturnTheUser()
        {
            // Arrange
            user.Role = expectedUserRole;
            context.Add(expectedUserRole);
            context.SaveChanges();
            
            // Act
            var actualUserRole = await repository.GetUserRoleByNameAsync(user.Role.Name);

            // Assert
            Assert.AreEqual(expectedUserRole, actualUserRole);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetUserRoleByNameAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetUserRoleByNameAsync(user.Role.Name);
        }
        
        [TestMethod]
        public async Task GetUsersAsync_ThereAreUsers_ShouldReturnThePaginatedUsers()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            expectedUser.IsDeleted = false;
            context.Add(expectedUser);
            await context.SaveChangesAsync();
            
            //Act
            var actualPaginatedUsers = await repository.GetUsersAsync(paging);

            //Assert
            Assert.AreEqual(expectedUser, actualPaginatedUsers.Data.First());
        }
        
        [TestMethod]
        public async Task GetUsersAsync_UserExistsFilterBy_ShouldReturnThePaginatedUsers()
        {
            // Arrange 
            paging = new PagingModel();
            paging.Order = "Id";
            
            paging.FilterBy = expectedUser.Name;
            context.Add(expectedUser);
            context.SaveChanges();
            
            //Act
            var actualPaginatedUsers = await repository.GetUsersAsync(paging);

            //Assert
            Assert.IsTrue(actualPaginatedUsers.Data.Count() == 1);
            Assert.IsTrue(actualPaginatedUsers.Counts.Total == 1);
            Assert.AreEqual(expectedUser, actualPaginatedUsers.Data.First());
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetUsersAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange 
            await context.DisposeAsync();
            
            //Act
            await repository.GetUsersAsync(paging);
        }
    }
}