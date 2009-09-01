namespace TrackerTools.Web
{
    using System.Web.Mvc;

    public class CreatedResult : ActionResult
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
}
