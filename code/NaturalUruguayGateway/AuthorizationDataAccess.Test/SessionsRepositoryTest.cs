using System.Threading.Tasks;
using NaturalUruguayGateway.AuthorizationDataAccess.Repositories;
using NaturalUruguayGateway.AuthorizationDataAccessInterface.Repositories;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.DataAccess.Context;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NaturalUruguayGateway.AuthorizationDataAccess.Test
{
    [TestClass]
    public class SessionsRepositoryTest
    {
        private IFixture fixture = null;
        private DbContextOptionsBuilder<NaturalUruguayContext> contextBuilderOptions = null;
        private NaturalUruguayContext context = null;
        private ISessionsRepository repository = null;
        private LoginModel login = null;
        private User user = null;
        private User expectedUser = null;
        private Session session = null;
        private Session expectedSession = null;
        private int expectedResult = 0;
        private const string DataBaseName = "NaturalUruguayDB";

        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            contextBuilderOptions = new DbContextOptionsBuilder<NaturalUruguayContext>();
            contextBuilderOptions.UseInMemoryDatabase(databaseName: DataBaseName);
            context = new NaturalUruguayContext(contextBuilderOptions.Options);
            context.Database.EnsureDeleted();
            user = fixture.Create<User>();
            expectedUser = fixture.Create<User>();
            session = fixture.Create<Session>();
            expectedSession = fixture.Create<Session>();
            expectedResult = fixture.Create<int>();
            login = fixture.Create<LoginModel>();
            
            repository = new SessionsRepository(context);
        }
        
        [TestMethod]
        public async Task GetUserAsync_UserExists_ShouldReturnTheUser()
        {
            // Arrange 
            login.Email = expectedUser.Email;
            login.Password = expectedUser.Password;
            context.Add(expectedUser);
            context.SaveChanges();
            
            //Act
            var actualUser = await repository.GetUserAsync(login);

            //Assert
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [TestMethod]
        public async Task GetUserAsync_UserNotExists_ShouldReturnNull()
        {
            // Arrange 
            login.Email = "no@email.com";
            
            //Act
            var actualUser = await repository.GetUserAsync(login);

            //Assert
            Assert.IsNull(actualUser);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetUserAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetUserAsync(login);
        }
        
        [TestMethod]
        public async Task GetSessionAsync_UserExists_ShouldReturnTheSession()
        {
            // Arrange 
            expectedSession.User = expectedUser;
            expectedSession.UserId = expectedUser.Id;
            context.Add(expectedSession);
            context.SaveChanges();
            
            //Act
            var actualSession = await repository.GetSessionAsync(expectedUser);

            //Assert
            Assert.AreEqual(expectedSession, actualSession);
        }
        
        [TestMethod]
        public async Task GetSessionAsync_UserNotExists_ShouldReturnNull()
        {
            // Arrange 

            //Act
            var actualSession = await repository.GetSessionAsync(expectedUser);

            //Assert
            Assert.IsNull(actualSession);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetSessionAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetSessionAsync(expectedUser);
        }
        
        [TestMethod]
        public async Task CreateSessionAsync_NoRestrictions_ShouldReturnTheSession()
        {
            // Arrange 
            context.Add(expectedSession);
            context.SaveChanges();
            
            //Act
            var actualSession = await repository.CreateSessionAsync(expectedUser);

            //Assert
            Assert.IsNotNull(actualSession);
            Assert.AreEqual(expectedUser.Id, actualSession.UserId);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task CreateSessionAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.CreateSessionAsync(expectedUser);
        }
        
        [TestMethod]
        public async Task LogoutAsync_SessionExists_ShouldReturnOne()
        {
            // Arrange 
            expectedResult = 1;
            context.Add(expectedSession);
            context.SaveChanges();
            
            //Act
            var actualResult = await repository.LogoutAsync(expectedSession.Token);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        
        [TestMethod]
        public async Task LogoutAsync_SessionNotExists_ShouldReturnZero()
        {
            // Arrange 
            expectedResult = 0;
            
            //Act
            var actualResult = await repository.LogoutAsync(expectedSession.Token);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task LogoutAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.LogoutAsync(session.Token);
        }

        [TestMethod]
        public async Task GetSessionByTokenAsync_UserExists_ShouldReturnTheSession()
        {
            // Arrange 
            context.Add(expectedSession);
            context.SaveChanges();
            
            //Act
            var actualSession = await repository.GetSessionByToken(expectedSession.Token);

            //Assert
            Assert.AreEqual(expectedSession, actualSession);
        }
        
        [TestMethod]
        public async Task GetSessionByTokenAsync_UserNotExists_ShouldReturnNull()
        {
            // Arrange 

            //Act
            var actualSession = await repository.GetSessionByToken(expectedSession.Token);

            //Assert
            Assert.IsNull(actualSession);
        }
        
        [ExpectedException(typeof(WrappedDbException))]
        [TestMethod]
        public async Task GetSessionByTokenAsync_ContextIsNull_ShouldThrownException()
        {
            // Arrange
            await context.DisposeAsync();
            
            //Act
            await repository.GetSessionByToken(expectedSession.Token);
        }
    }
}