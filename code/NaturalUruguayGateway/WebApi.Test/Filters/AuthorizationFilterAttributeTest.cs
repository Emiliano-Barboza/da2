using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.AuthorizationEngineInterface.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.HelperInterface.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NaturalUruguayGateway.WebApi.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace NaturalUruguayGateway.WebApi.Test
{
    [TestClass]
    public class AuthorizationFilterAttributeTest
    {
        private const string TokenRequiredError = "Access Token Required";
        private const string SchemaError = "Invalid Schema";
        private const string TokenError = "Invalid Token";
        private const string AccessError = "Insuficient privileges";
        private IFixture fixture = null;
        private AuthorizationFilterAttribute authorizationFilter = null;
        private Mock<HttpContext> moqHttpContext = null;
        private Mock<IServiceProvider> moqServiceProvider = null;
        private Mock<ISessionsService> moqSessionsService = null;
        private Mock<IConfigurationManager> moqConfiguration = null;
        private Mock<ActionDescriptor> moqActionDescriptor = null;
        private AuthorizationFilterContext authorizationContext = null;
        private ActionContext actionContext = null;
        private IActionResult expectedActionResult = null;
        private string token = null;
        private const string TokenSchema = "Bearer ";
        private const string SuperAdminRole = "Super admin";
        private string authorizationHdeaderKey = null;
        private string url = null;
        private string tracerId = null;
        private Session expectedSession = null;
        private List<IFilterMetadata> filtersMetadata = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Inject(new UriScheme("http"));
            authorizationHdeaderKey = "Authorization";
            token = TokenSchema + "SomeRandomToken";
            expectedSession = fixture.Create<Session>();
            tracerId = fixture.Create<string>();
            url = "//autofixture.com";
            moqSessionsService = new Mock<ISessionsService>(MockBehavior.Strict);
            moqConfiguration = new Mock<IConfigurationManager>(MockBehavior.Strict);
            moqConfiguration.SetupGet(x => x.AuthenticationSchema).Returns(TokenSchema);
            moqSessionsService.Setup(x => x.GetSessionByTokenAsync(It.IsAny<string>())).Returns(Task.FromResult(expectedSession));
            moqServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            moqServiceProvider.Setup(x => x.GetService(typeof(ISessionsService))).Returns(moqSessionsService.Object);
            moqServiceProvider.Setup(x => x.GetService(typeof(IConfigurationManager))).Returns(moqConfiguration.Object);
            moqHttpContext = new Mock<HttpContext>(MockBehavior.Default);
            moqHttpContext.Setup(x => x.Request.Headers[authorizationHdeaderKey]).Returns(token);
            moqHttpContext.Setup(x => x.Request.Path).Returns(url);
            moqHttpContext.Setup(x => x.Request.HttpContext.TraceIdentifier).Returns(tracerId);
            moqHttpContext.Setup(x => x.RequestServices).Returns(moqServiceProvider.Object);
            
            
            moqActionDescriptor = new Mock<ActionDescriptor>();
            actionContext = new ActionContext(moqHttpContext.Object, new RouteData(), moqActionDescriptor.Object);
            filtersMetadata = new List<IFilterMetadata>();
            authorizationContext = new AuthorizationFilterContext(actionContext, filtersMetadata);
            authorizationFilter = new AuthorizationFilterAttribute();
        }

        [TestMethod]
        public void OnAuthorization_UserRoleHasAccess_ShouldReturnSuccess()
        {
            // Arrange
            expectedSession.User.Role = new UserRole()
            {
                Name = SuperAdminRole
            };
            authorizationFilter.Roles = new[] { SuperAdminRole };

            // Act
            authorizationFilter.OnAuthorization(authorizationContext);
            var actualActionResult = authorizationContext.Result;

            // Assert
            Assert.AreEqual(expectedActionResult, actualActionResult);
            Assert.IsNull(actualActionResult);
        }

        [TestMethod]
        public void OnAuthorization_IsAnonymous_ShouldReturnSuccess()
        {
            // Arrange
            var allowAnonymousFilter = new AllowAnonymousFilter();
            filtersMetadata.Add(allowAnonymousFilter);

            // Act
            authorizationFilter.OnAuthorization(authorizationContext);
            var actualActionResult = authorizationContext.Result;

            // Assert
            Assert.AreEqual(expectedActionResult, actualActionResult);
            Assert.IsNull(actualActionResult);
        }

        [TestMethod]
        public void OnAuthorization_InvalidAuthorizationHeader_ShouldReturnUnauthorizedObjectResult()
        {
            // Arrange
            token = null;
            moqHttpContext.Setup(x => x.Request.Headers[authorizationHdeaderKey]).Returns(token);
            expectedActionResult = new ObjectResult(TokenRequiredError);

            // Act
            authorizationFilter.OnAuthorization(authorizationContext);
            var actualActionResult = authorizationContext.Result;

            // Assert
            Assert.AreEqual(expectedActionResult.GetType(), actualActionResult.GetType());
            Assert.AreEqual(expectedActionResult.ToString(), actualActionResult.ToString());
        }

        [TestMethod]
        public void OnAuthorization_InvalidSchema_ShouldReturnUnauthorizedObjectResult()
        {
            // Arrange
            token = "no schema here";
            moqHttpContext.Setup(x => x.Request.Headers[authorizationHdeaderKey]).Returns(token);
            expectedActionResult = new ObjectResult(SchemaError);

            // Act
            authorizationFilter.OnAuthorization(authorizationContext);
            var actualActionResult = authorizationContext.Result;

            // Assert
            Assert.AreEqual(expectedActionResult.GetType(), actualActionResult.GetType());
            Assert.AreEqual(expectedActionResult.ToString(), actualActionResult.ToString());
        }

        [TestMethod]
        public void OnAuthorization_UserWithoutSession_ShouldReturnUnauthorizedObjectResult()
        {
            // Arrange
            expectedSession = null;
            moqSessionsService.Setup(x => x.GetSessionByTokenAsync(It.IsAny<string>())).Returns(Task.FromResult(expectedSession));
            expectedActionResult = new ObjectResult(TokenError);

            // Act
            authorizationFilter.OnAuthorization(authorizationContext);
            var actualActionResult = authorizationContext.Result;

            // Assert
            Assert.AreEqual(expectedActionResult.GetType(), actualActionResult.GetType());
        }

        [TestMethod]
        public void OnAuthorization_UserWithoutRoleAccess_ShouldReturnUnauthorizedResult()
        {
            // Arrange
            expectedActionResult = new ObjectResult(AccessError);

            // Act
            authorizationFilter.OnAuthorization(authorizationContext);
            var actualActionResult = authorizationContext.Result;

            // Assert
            Assert.AreEqual(expectedActionResult.GetType(), actualActionResult.GetType());
        }
    }
}