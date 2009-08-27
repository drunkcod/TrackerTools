using System;
using System.Net;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace TrackerTools.Web
{
    public class XmlInputAttribute : FilterAttribute, IActionFilter
    {
        private readonly string parameterName;

        public XmlInputAttribute(string parameterName) { this.parameterName = parameterName; }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var serializer = new XmlSerializer(filterContext.ActionParameters[parameterName].GetType());
            try {
                filterContext.ActionParameters[parameterName] = serializer.Deserialize(filterContext.HttpContext.Request.InputStream);
            } catch (InvalidOperationException) {
                filterContext.Result = new ErrorResult(HttpStatusCode.BadRequest, new ErrorResponse("Error parsing payload."));
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) { }
    }
}
