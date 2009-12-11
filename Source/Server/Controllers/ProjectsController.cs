using System.Web.Mvc;
using TrackerTools;
using TrackerTools.Web;

namespace TrackerTools.Server.Controllers
{
    public class ProjectsController : Controller
    {
        [Post, XmlInput("project")]
        public ActionResult Index(TrackerProject project){
            return new CreatedResult("1", new XmlResult(project));
        }
    }
}
