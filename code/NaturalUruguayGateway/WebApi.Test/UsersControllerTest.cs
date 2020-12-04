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
    public class UsersControllerTest : BaseControllerTest
    {
        private User expectedUser = null;
        private IEnumerable<User> expectedUsers = null;
        private User user = null;
        private PagingModel paging = null;
        private PaginatedModel<User> paginatedUsers = null;
        private int userId = 0;
        private Mock<IUsersService> moqService = null;
        private UsersController controller = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            InitializeComponents();
            moqService = new Mock<IUsersService>(MockBehavior.Strict);
            controller = new UsersController(moqService.Object);
            controller.ControllerContext = controllerContext;
            expectedUser = fixture.Create<User>();
            expectedUsers = fixture.CreateMany<User>();
            paging = fixture.Create<PagingModel>();
            paginatedUsers = fixture.Create<PaginatedModel<User>>();
            user = fixture.Create<User>();
            userId = fixture.Create<int>();
        }
        
        [TestMethod]
        public async Task AddUserAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.AddUserAsync(It.IsAny<User>())).ReturnsAsync(expectedUser);
            
            // Act
            var response = await controller.AddUserAsync(user) as ObjectResult;
            var actualUser = response.Value;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedUser, actualUser);
        }

        [TestMethod]
        public async Task AddUserAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.AddUserAsync(user)).Throws<Exception>();

            // Act
            var response = await controller.AddUserAsync(user);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task DeleteUserAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.DeleteUserByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
            
            // Act
            var response = await controller.DeleteUserAsync(userId) as ObjectResult;
            var actualUser = response.Value;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [TestMethod]
        public async Task DeleteUserAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.DeleteUserByIdAsync(userId)).Throws<Exception>();

            // Act
            var response = await controller.DeleteUserAsync(userId);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task UpdateUserAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(expectedUser);
            
            // Act
            var response = await controller.UpdateUserAsync(userId, user) as ObjectResult;
            var actualUser = response.Value;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedUser, actualUser);
        }

        [TestMethod]
        public async Task UpdateUserAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.UpdateUserAsync(user)).Throws<Exception>();

            // Act
            var response = await controller.UpdateUserAsync(userId, user);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetUsersAsync_NoRestrictions_Success()
        {
            // Arrange
            paginatedUsers.Data = expectedUsers;
            paginatedUsers.Counts = new PaginatedCountModel(paging, expectedUsers.Count());
            moqService.Setup(x => x.GetUsersAsync(It.IsAny<PagingModel>())).ReturnsAsync(paginatedUsers);
            
            // Act
            var response = await controller.GetUsersAsync(paging) as ObjectResult;
            var actualPaginatedUsers = response.Value as PaginatedModel<User>;
            
            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actualPaginatedUsers, typeof(PaginatedModel<User>));
            Assert.AreEqual(expectedUsers, actualPaginatedUsers.Data);
            Assert.IsNotNull(actualPaginatedUsers.Counts);
            Assert.AreEqual(paging, actualPaginatedUsers.Counts.Paging);
            Assert.AreEqual(expectedUsers.Count(), actualPaginatedUsers.Counts.Total);
        }
        
        [TestMethod]
        public async Task GetUsersAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.GetUsersAsync(It.IsAny<PagingModel>())).Throws<Exception>();

            // Act
            var response = await controller.GetUsersAsync(paging);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        
        [TestMethod]
        public async Task GetUserAsync_NoRestrictions_Success()
        {
            // Arrange
            moqService.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(expectedUser);
            
            // Act
            var response = await controller.GetUserAsync(userId) as ObjectResult;
            var actualUser = response.Value as User;
            
            // Assert
            moqService.VerifyAll();
            Assert.AreEqual(expectedUser, actualUser);
        }
        
        [TestMethod]
        public async Task GetUserAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.GetUserAsync(It.IsAny<int>())).Throws<Exception>();

            // Act
            var response = await controller.GetUserAsync(userId);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
    }
}