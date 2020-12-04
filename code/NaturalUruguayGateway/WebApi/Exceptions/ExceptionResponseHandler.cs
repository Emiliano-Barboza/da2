using System;
using System.Net;
using System.Security.Authentication;
using NaturalUruguayGateway.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace NaturalUruguayGateway.WebApi.Exceptions
{
    public static class ExceptionResponseHandler
    {
        private static ResponseProblemDetails GetDefaultResponse(string messageError, HttpRequest request)
        {
            var problemDetails = new ResponseProblemDetails()
            {
                Type = request.GetDisplayUrl(),
                Detail = messageError,
                Title = "There was an error on the data provided.",
                TraceId = request.HttpContext.TraceIdentifier
            };
            return problemDetails;
        }
        public static ObjectResult GetExceptionResponse(Exception exception, HttpRequest request)
        {
            var problemDetails = GetDefaultResponse(exception.Message, request);
            
            if (exception is WrappedDbException)
            {
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "There was an error on database query.";
            }
            else if (exception is InvalidCredentialException)
            {
                problemDetails.Status = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
            }
            
            var response = new ObjectResult(problemDetails);
            response.StatusCode = problemDetails.Status;
            return response;
        }
        
        public static ObjectResult GetUnauthorizedResponse(string messageError, HttpRequest request)
        {
            var problemDetails = GetDefaultResponse(messageError, request);
            problemDetails.Status = (int)HttpStatusCode.Unauthorized;
            var response = new ObjectResult(problemDetails);
            response.StatusCode = problemDetails.Status;
            return response;
        }
    }
}