using System;
using System.IO;
using System.Web;
using System.Web.Routing;

namespace TrackerTools.Web
{
    public class BasicHttpResponse : HttpResponseBase
    {
        string contentType;
        int statusCode = 200;
        Stream outputStream = new MemoryStream();

        public static BasicHttpResponse From(HttpRequestBase request, RouteCollection routes){
            var response = new BasicHttpResponse();
            var context = new BasicContext(request, response);
            var route = routes.GetRouteData(context);
            var handler = route.RouteHandler.GetHttpHandler(new RequestContext(context, route));

            handler.ProcessRequest(context.ToHttpContext());
            return response;
        }

        public override void Write(string s){
            Console.WriteLine(s);
        }

        public override string ContentType {
            get { return contentType; }
            set { contentType = value; }
        }

        public override int StatusCode {
            get { return statusCode; }
            set { statusCode = value; }
        }

        public override System.IO.Stream OutputStream {
            get { return outputStream; }
        }

        public string Body {
            get {
                var reader = new StreamReader(OutputStream);
                outputStream.Position = 0;
                return reader.ReadToEnd();
            }
        }
    }
}