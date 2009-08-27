using System.Net;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace TrackerTools.Web
{
    [XmlRoot("Error")]
    public class ErrorResponse
    {
        public ErrorResponse() : this(string.Empty) { }
        public ErrorResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    public class ErrorResult : ActionResult
    {
        readonly HttpStatusCode statusCode;
        readonly ErrorResponse errorResponse;

        public ErrorResult(HttpStatusCode statusCode, ErrorResponse errorResponse)
        {
            this.statusCode = statusCode;
            this.errorResponse = errorResponse;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            new XmlResult(errorResponse).ExecuteResult(context);
        }
    }
}
