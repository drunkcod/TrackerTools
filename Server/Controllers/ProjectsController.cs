namespace Server.Controllers
{
    using System;
    using System.Web.Mvc;
    using TrackerTools.Web;

    class CreatedResult : ActionResult
    {
        readonly string location;
        readonly ActionResult innerResult;

        public CreatedResult(string location, ActionResult innerResult) 
        {
            this.location = location;
            this.innerResult = innerResult; 
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 201;
            response.AddHeader("Location", context.HttpContext.Request.Url + location);
            innerResult.ExecuteResult(context);
        }
    }

    public class ProjectsController : Controller
    {
        public ActionResult Index()
        {
            
            return new CreatedResult("1", new XmlResult("Hello World!"));
        }
    }
}
