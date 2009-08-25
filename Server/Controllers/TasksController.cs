using System;
using System.Web.Mvc;
using TrackerTools.Web;

namespace Server.Controllers
{
    public class Task
    {
        public string Description { get; set; }
    }

    public class TasksController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [AcceptVerbs(HttpVerbs.Post),  XmlInput("value")]
        public ActionResult Index(Task value)
        {
            return new XmlResult(value);
        }

    }
}
