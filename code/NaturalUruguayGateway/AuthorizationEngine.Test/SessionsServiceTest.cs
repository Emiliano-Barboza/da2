using System.IO;
using System.Net.Mail;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.AuthorizationDataAccessInterface.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.AuthorizationEngine.Services;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.HelperInterface.Services;

namespace NaturalUruguayGateway.AuthorizationEngine.Test
{
    [TestClass]
    public class SessionsServiceTest
    {
        private IFixture fixture = null;
        private Session expectedSession = null;
        private User expectedUser = null;
        private LoginModel login = null;
        private Mock<ISessionsRepository> moqRepository = null;
        private Mock<IConfigurationManager> moqConfiguration = null;
        private Mock<IEncryptor> moqEncryptor = null;
        private string token = null;
        private string schema = null;
        private string expectedUrl = null;
        private string expectedPassword = null;
        private SessionsService service = null;

        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<User>(c => c
                .With(x =>
                        x.Email,
                    fixture.Create<MailAddress>().Address));

            expectedSession = fixture.Create<Session>();
            expectedUser = fixture.Create<User>();
            login = fixture.Create<LoginModel>();
            moqRepository = new Mock<ISessionsRepository>(MockBehavior.Strict);
            moqConfiguration = new Mock<IConfigurationManager>(MockBehavior.Strict);
            moqEncryptor = new Mock<IEncryptor>(MockBehavior.Strict);
            moqEncryptor.Setup(x => x.EncryptAsync(It.IsAny<string>())).ReturnsAsync(expectedPassword);
            token = fixture.Create<string>();
            schema = fixture.Create<string>();
            expectedPassword = fixture.Create<string>();
            expectedUrl = "google.com";
            service = new SessionsService(moqRepository.Object, moqConfiguration.Object, moqEncryptor.Object);
        }

        [TestMethod]
        public async Task LoginAsync_UserExists_ShouldReturnTheSession()
        {
            // Arrange
            moqRepository.Setup(x => x.GetUserAsync(It.IsAny<LoginModel>())).ReturnsAsync(expectedUser);
            moqRepository.Setup(x => x.GetSessionAsync(expectedUser)).ReturnsAsync(expectedSession);
                
            // Act
            Session actualSession = await service.LoginAsync(login);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedSession, actualSession);
        }

        [ExpectedException(typeof(InvalidCredentialException))]
        [TestMethod]
        public async Task LoginAsync_UserNotExists_ShouldThrowException()
        {
            // Arrange
            expectedUser = null;
            moqRepository.Setup(x => x.GetUserAsync(It.IsAny<LoginModel>())).ReturnsAsync(expectedUser);

            // Act
            await service.LoginAsync(login);
        }

        [TestMethod]
        public async Task LoginAsync_UserSessionNotExists_ShouldReturnNewSession()
        {
            // Arrange
            Session nullSession = null;
            moqRepository.Setup(x => x.GetUserAsync(It.IsAny<LoginModel>())).ReturnsAsync(expectedUser);
            moqRepository.Setup(x => x.GetSessionAsync(It.IsAny<User>())).ReturnsAsync(nullSession);
            moqRepository.Setup(x => x.CreateSessionAsync(It.IsAny<User>())).ReturnsAsync(expectedSession);
                
            // Act
            Session actualSession = await service.LoginAsync(login);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(expectedSession, actualSession);
        }
        
        [TestMethod]
        public async Task LogoutAsync_WithToken_Success()
        {
            // Arrange
            moqConfiguration.SetupGet(x => x.LogoutRedirectUrl).Returns(expectedUrl);
            moqConfiguration.SetupGet(x => x.AuthenticationSchema).Returns(schema);
            moqRepository.Setup(x => x.LogoutAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<int>());
                
            // Act
            string actualUrl = await service.LogoutAsync(token);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(actualUrl, expectedUrl);
        }
        
        [ExpectedException(typeof(InvalidCredentialException))]
        [TestMethod]
        public async Task LogoutAsync_WithoutToken_ShouldThrowException()
        {
            // Arrange
            token = null;
            moqConfiguration.SetupGet(x => x.LogoutRedirectUrl).Returns(expectedUrl);
            moqConfiguration.SetupGet(x => x.AuthenticationSchema).Returns(schema);
            moqRepository.Setup(x => x.LogoutAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<int>());
                
            // Act
            string actualUrl = await service.LogoutAsync(token);
        }
        
        [TestMethod]
        public async Task GetSessionByTokenAsync_Success()
        {
            // Arrange
            moqRepository.Setup(x => x.GetSessionByToken(It.IsAny<string>()))
                .ReturnsAsync(expectedSession);
                
            // Act
            var actualSession = await service.GetSessionByTokenAsync(token);

            // Assert
            moqRepository.VerifyAll();
            Assert.AreEqual(actualSession, expectedSession);
        }
    }
}