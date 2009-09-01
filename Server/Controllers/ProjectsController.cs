namespace Server.Controllers
{
    using System;
    using System.Web.Mvc;
    using TrackerTools.Web;

    public class ProjectsController : Controller
    {
        public ActionResult Index()
        {
            
            return new CreatedResult("1", new XmlResult("Hello World!"));
        }
    }
}
