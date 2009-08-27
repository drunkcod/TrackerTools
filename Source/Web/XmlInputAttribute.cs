using System;
using System.Xml.Serialization;
using System.Web.Mvc;
using System.Net;

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
                filterContext.Result = new ErrorResult(HttpStatusCode.BadRequest, new Error("Error parsing payload."));
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) { }
    }
}
