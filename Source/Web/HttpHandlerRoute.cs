namespace TrackerTools.Web
{
    using System;
    using System.Web;
    using System.Web.Routing;

    public class HttpHandlerRoute : IRouteHandler
    {
        class MissingHttpHandler : IHttpHandler 
        {
            private MissingHttpHandler(){}
            public static readonly IHttpHandler Instance = new MissingHttpHandler();
            public bool IsReusable { get { return false; } }
            public void ProcessRequest(HttpContext context){}
        }

        Func<IHttpHandler> createHandler;
        IHttpHandler handler = MissingHttpHandler.Instance;

        public HttpHandlerRoute(Func<IHttpHandler> createHandler)
        {
            this.createHandler = createHandler;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (handler.IsReusable)
                return handler;
            return handler = createHandler();
        }
    }
}
