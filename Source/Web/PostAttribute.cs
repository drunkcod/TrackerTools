using System.Reflection;
using System.Web.Mvc;


namespace TrackerTools.Web
{
    public class PostAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo) {
            return controllerContext.HttpContext.Request.HttpMethod == "POST";
        }
    }
}
