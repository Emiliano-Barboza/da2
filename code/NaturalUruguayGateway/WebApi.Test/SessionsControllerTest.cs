using System;
using System.Threading.Tasks;
using NaturalUruguayGateway.AuthorizationEngineInterface.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.WebApi.Controllers;

namespace NaturalUruguayGateway.WebApi.Test
{
    [TestClass]
    public class SessionsControllerTest : BaseControllerTest
    {
        private Session expectedSession = null;
        private LoginModel login = null;
        private Mock<ISessionsService> moqService = null;
        private SessionsController controller = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            InitializeComponents();
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            expectedSession = fixture.Create<Session>();
            login = fixture.Create<LoginModel>();
            moqService = new Mock<ISessionsService>(MockBehavior.Strict);
            controller = new SessionsController(moqService.Object);
            controller.ControllerContext = controllerContext;
        }
        
        [TestMethod]
        public async Task LoginAsync_Success()
        {
            // Arrange
            moqService.Setup(x => x.LoginAsync(login)).ReturnsAsync(expectedSession);
            
            // Act
            var response = await controller.LoginAsync(login) as ObjectResult;
            var ActualSession = response.Value;

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(expectedSession, ActualSession);
        }

        [TestMethod]
        public async Task LoginAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.LoginAsync(login)).Throws<Exception>();
            
            // Act
            var response = await controller.LoginAsync(login);

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }

        [TestMethod]
        public async Task LogoutAsync_Success()
        {
            // Arrange
            var url = "http://example.com";
            moqService.Setup(x => x.LogoutAsync(It.IsAny<string>())).ReturnsAsync(url);

            // Act
            var response = await controller.LogoutAsync();

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }
        
        [TestMethod]
        public async Task LogoutAsync_IfThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            moqService.Setup(x => x.LogoutAsync(It.IsAny<string>())).Throws<Exception>();
            
            // Act
            var response = await controller.LogoutAsync();

            // Assert
            moqService.VerifyAll();
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
    }
}