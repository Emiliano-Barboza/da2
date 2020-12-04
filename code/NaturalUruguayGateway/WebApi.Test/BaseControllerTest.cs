using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace NaturalUruguayGateway.WebApi.Test
{
    public class BaseControllerTest
    {
        private const string TokenSchema = "Bearer ";
        protected IFixture fixture = null;
        protected Mock<HttpContext> moqHttpContext = null;
        protected Mock<ControllerActionDescriptor> moqActionDescriptor = null;
        protected ActionContext moqActionContext = null;
        protected ControllerContext controllerContext = null;
        protected string url = null;
        protected string tracerId = null;
        protected string authorizationHdeaderKey = null;
        private string token = null;

        protected void InitializeComponents()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            tracerId = fixture.Create<string>();
            authorizationHdeaderKey = "Authorization";
            token = TokenSchema + "SomeRandomToken";
            url = "//autofixture.com";
            moqHttpContext = new Mock<HttpContext>(MockBehavior.Default);
            moqHttpContext.Setup(x => x.Request.Path).Returns(url);
            moqHttpContext.Setup(x => x.Request.HttpContext.TraceIdentifier).Returns(tracerId);
            moqHttpContext.Setup(x => x.Request.Headers[authorizationHdeaderKey]).Returns(token);
            
            moqActionDescriptor = new Mock<ControllerActionDescriptor>();
            moqActionContext = new ActionContext(moqHttpContext.Object, new RouteData(), moqActionDescriptor.Object);
            
            controllerContext = new ControllerContext(moqActionContext);
        }
    }
}