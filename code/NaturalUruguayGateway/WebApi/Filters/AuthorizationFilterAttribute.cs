using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using NaturalUruguayGateway.AuthorizationEngineInterface.Services;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.HelperInterface.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using NaturalUruguayGateway.WebApi.Exceptions;

namespace NaturalUruguayGateway.WebApi.Filters
{
    public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        private const string AuthorizationHeader = "Authorization";
        private const string TokenRequiredError = "Access Token Required";
        private const string SchemaError = "Invalid Schema";
        private const string TokenError = "Invalid Token";
        private const string AccessError = "Insuficient privileges";
        
        public string[] Roles { get; set; }

        public AuthorizationFilterAttribute()
        {
            Roles = new string[0];
        }
        
        private bool HasAnonymousAuthentication(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = false;
            if (context.Filters != null)
            {
                hasAllowAnonymous = context.Filters.Any(item => item is IAllowAnonymousFilter);
            }
            return hasAllowAnonymous;
        }
        
        private string GetAuthorizationHeader(AuthorizationFilterContext context)
        {
            var authorizationHeader = string.Empty;
            var httpContext = context.HttpContext;
            var request = httpContext?.Request;
            if (request != null)
            {
                authorizationHeader = request.Headers[AuthorizationHeader];
            }
            return authorizationHeader;
        }

        private bool IsValidAuthorizationValue(string value)
        {
            var isValid = !string.IsNullOrWhiteSpace(value);
            return isValid;
        }
        
        private bool HasValidAuthorizationHeader(AuthorizationFilterContext context)
        {
            var authorization = GetAuthorizationHeader(context);
            var hasValidHeader = IsValidAuthorizationValue(authorization);
            return hasValidHeader;
        }

        private string GetTokenSchema(AuthorizationFilterContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var configuration = (IConfigurationManager)serviceProvider.GetService(typeof(IConfigurationManager));
            var schema = configuration.AuthenticationSchema;
            return schema;
        }
        
        private bool HasValidSchema(AuthorizationFilterContext context)
        {
            var authorization = GetAuthorizationHeader(context);
            var isValid = IsValidAuthorizationValue(authorization);
            if (isValid)
            {
                var schema = GetTokenSchema(context);
                isValid = !string.IsNullOrWhiteSpace(schema) && authorization.Contains(schema, StringComparison.InvariantCultureIgnoreCase);
            }
            
            return isValid;
        }
        
        private bool IsInRole(Session session)
        {
            bool isRole = false;
            if (session != null && session.User != null)
            {
                isRole = Roles.Any(x => x == session.User.Role.Name);
            }
            return isRole;
        }
        
        private Session GetSession(AuthorizationFilterContext context)
        {
            Session session = null;
            var serviceProvider = context.HttpContext.RequestServices;
            var service = (ISessionsService)serviceProvider.GetService(typeof(ISessionsService));
            if (service != null)
            {
                var token = GetAuthorizationHeader(context);
                if (IsValidAuthorizationValue(token))
                {
                    var schema = GetTokenSchema(context);
                    token = token.Replace(schema, "");
                    var sessionTask = service.GetSessionByTokenAsync(token);
                    session = sessionTask.Result;
                }
            }
            
            return session;
        }

        private void CheckAccessToken(AuthorizationFilterContext context)
        {
            ObjectResult result = null;
            
            if (!HasValidAuthorizationHeader(context))
            {
                result = ExceptionResponseHandler.GetUnauthorizedResponse(TokenRequiredError, context.HttpContext.Request);
                context.Result = result;
            } else if (!HasValidSchema(context))
            {
                result = ExceptionResponseHandler.GetUnauthorizedResponse(SchemaError, context.HttpContext.Request);
                context.Result = result;
            }
            else
            {
                var session = GetSession(context);

                if (session == null)
                {
                    result = ExceptionResponseHandler.GetUnauthorizedResponse(TokenError, context.HttpContext.Request);
                    context.Result = result;
                }
                else if (!IsInRole(session))
                {
                    result = ExceptionResponseHandler.GetUnauthorizedResponse(AccessError, context.HttpContext.Request);
                    context.Result = result;
                }
            }
        }
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = HasAnonymousAuthentication(context);
            
            if (!hasAllowAnonymous)
            {
                CheckAccessToken(context);
            }
        }
    }
}