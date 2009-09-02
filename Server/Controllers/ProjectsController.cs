namespace Server.Controllers
{
    using TrackerTools;
    using TrackerTools.Web;
    using System.Web.Mvc;

    public class ProjectsController : Controller
    {
        [XmlInput("project"), AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(TrackerProject project)
        {            
            return new CreatedResult("1", new XmlResult(project));
        }
    }
}
