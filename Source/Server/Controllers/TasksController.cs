namespace TrackerTools.Server.Cotnrollers
{
    using System;
    using System.Web.Mvc;
    using TrackerTools.Web;

    public class Task
    {
        public string Description { get; set; }
    }

    public class TasksController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult { Content = "Hello Tracker World!" };
        }

        [AcceptVerbs(HttpVerbs.Post),  XmlInput("value")]
        public ActionResult Index(Task value)
        {
            return new XmlResult(value);
        }

    }
}
