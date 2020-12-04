using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.HelperInterface.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngine.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Test
{
    [TestClass]
    public class UsersServiceTest
    {
        private IFixture fixture = null;
        private User expectedUser = null;
        private IEnumerable<User> expectedUsers = null;
        private User user = null;
        private User otherUser = null;
        private UserRole adminUserRole = null;
        private int userId = 0;
        private string defaultPassword = "defaultPassword";
        private PagingModel paging = null;
        private PaginatedModel<User> paginatedUsers = null;
        private Mock<IUsersRepository> moqRepository = null;
        private Mock<IConfigurationManager> moqConfiguration = null;
        private Mock<IEncryptor> moqEncryptor = null;
        private string encryptedPassword = "encryptedPassword";
        private UsersService service = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            expectedUser = fixture.Create<User>();
            expectedUsers = fixture.CreateMany<User>();
            paging = fixture.Create<PagingModel>();
            paginatedUsers = fixture.Create<PaginatedModel<User>>();
            user = fixture.Create<User>();
            otherUser = fixture.Create<User>();
            userId = fixture.Create<int>();
            adminUserRole = new UserRole()
            {
                Name = "Admin"
            };
            moqRepository = new Mock<IUsersRepository>(MockBehavior.Strict);
            moqConfiguration = new Mock<IConfigurationManager>(MockBehavior.Strict);
            moqConfiguration.Setup(x => x.DefaultPassword).Returns(defaultPassword);
            moqEncryptor = new Mock<IEncryptor>(MockBehavior.Strict);
            moqEncryptor.Setup(x => x.EncryptAsync(It.IsAny<string>())).ReturnsAsync(encryptedPassword);
            service = new UsersService(moqRepository.Object, moqConfiguration.Object, moqEncryptor.Object);
        }
        
        [TestMethod]
        public async Task AddUserAsync_UserNotExistsPreviously_ShouldReturnTheNewUser()
        {
            // Arrange
            User existUser = null;
            moqRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(existUser);
            moqRepository.Setup(x => x.GetUserRoleByNameAsync(It.IsAny<string>())).ReturnsAsync(adminUserRole);
            moqRepository.Setup(x => x.AddUserAsync(It.IsAny<User>())).ReturnsAsync(expectedUser);

            // Act
            User actualUser = await service.AddUserAsync(user);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [ExpectedException(typeof(DuplicateNameException))]
        [TestMethod]
        public async Task AddUserAsync_UserExistPreviously_ShouldThrowException()
        {
            // Arrange
            moqRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(expectedUser);
                
            // Act
            await service.AddUserAsync(user);
        }
        
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task AddUserAsync_UserNotExistsAndUserRoleNotExists_ShouldThrowException()
        {
            // Arrange
            expectedUser = null;
            adminUserRole = null;
            moqRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(expectedUser);
            moqRepository.Setup(x => x.GetUserRoleByNameAsync(It.IsAny<string>())).ReturnsAsync(adminUserRole);
                
            // Act
            await service.AddUserAsync(user);
        }
        
        [TestMethod]
        public async Task DeleteUserByIdAsync_UserExists_ShouldReturnTheDeletedUser()
        {
            // Arrange
            moqRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
            moqRepository.Setup(x => x.DeleteUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
                
            // Act
            var actualUser = await service.DeleteUserByIdAsync(userId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedUser, actualUser);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task DeleteUserByIdAsync_UserNotExists_ShouldThrowException()
        {
            // Arrange
            expectedUser = null;
            moqRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
                
            // Act
            await service.DeleteUserByIdAsync(userId);
        }
        
        [TestMethod]
        public async Task UpdateUserAsync_UserExists_ShouldReturnTheUpdatedUser()
        {
            // Arrange
            otherUser = null;
            moqRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
            moqRepository.Setup(x => x.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(expectedUser);
            moqRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(otherUser);
                
            // Act
            User actualUser = await service.UpdateUserAsync(user);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [ExpectedException(typeof(DuplicateNameException))]
        [TestMethod]
        public async Task UpdateUserAsync_UserExistsAndTriesToChangeEmailThatAlreadyExists_ShouldThrowException()
        {
            // Arrange
            expectedUser.Email = otherUser.Email;
            moqRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
            moqRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(otherUser);
            moqRepository.Setup(x => x.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(expectedUser);
                
            // Act
            await service.UpdateUserAsync(user);
        }
        
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task UpdateUserAsync_UserNotExists_ShouldThrowException()
        {
            // Arrange
            expectedUser = null;
            moqRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
                
            // Act
            await service.UpdateUserAsync(user);
        }
        
        [TestMethod]
        public async Task GetUsersAsync_NoRestrictions_ShouldReturnTheUsers()
        {
            // Arrange
            paginatedUsers.Data = expectedUsers;
            paginatedUsers.Counts = new PaginatedCountModel(paging, expectedUsers.Count());
            moqRepository.Setup(x => x.GetUsersAsync(It.IsAny<PagingModel>())).ReturnsAsync(paginatedUsers);
                
            // Act
            PaginatedModel<User> actualPaginatedUsers = await service.GetUsersAsync(paging);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(paginatedUsers, actualPaginatedUsers);
        }
        
        [TestMethod]
        public async Task GetUserAsync_NoRestrictions_ShouldReturnTheUser()
        {
            // Arrange
            moqRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
                
            // Act
            var actualUser = await service.GetUserAsync(userId);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedUser, actualUser);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task GetUserAsync_UserNotExists_ShouldThrowException()
        {
            // Arrange
            expectedUser = null;
            moqRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
                
            // Act
            await service.GetUserAsync(userId);
        }
    }
}