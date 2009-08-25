using System;
using System.Xml.Serialization;
using System.Web.Mvc;
using System.Net;

namespace TrackerTools.Web
{
    public class Error
    {
        public Error() : this(string.Empty) { }
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    class ErrorResult : ActionResult
    {
        readonly HttpStatusCode statusCode;
        readonly Error error;

        public ErrorResult(HttpStatusCode statusCode, Error error)
        {
            this.statusCode = statusCode;
            this.error = error;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            new XmlResult(error).ExecuteResult(context);
        }
    }
}
