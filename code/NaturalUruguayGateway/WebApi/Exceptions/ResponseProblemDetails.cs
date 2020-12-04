using Microsoft.AspNetCore.Mvc;

namespace NaturalUruguayGateway.WebApi.Exceptions
{
    public class ResponseProblemDetails :  ProblemDetails
    {
        protected const string RFCSource = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        public string TraceId { get; set; }
        
        public ResponseProblemDetails()
        {
            Type = RFCSource;
        }
    }
}