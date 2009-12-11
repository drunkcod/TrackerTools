using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Routing;

namespace TrackerTools.Web
{
    public class BasicHttpResponse : HttpResponseBase
    {
        int statusCode = 200;
        Stream outputStream = new MemoryStream();
        NameValueCollection headers = new NameValueCollection();

        public static BasicHttpResponse From(HttpRequestBase request, RouteCollection routes){
            var response = new BasicHttpResponse();
            var context = new BasicContext(request, response);
            var route = routes.GetRouteData(context);
            var handler = route.RouteHandler.GetHttpHandler(new RequestContext(context, route));

            handler.ProcessRequest(context.ToHttpContext());
            return response;
        }

        public override string ContentType { get; set; }

        public override int StatusCode {
            get { return statusCode; }
            set { statusCode = value; }
        }

        public override Stream OutputStream {
            get { return outputStream; }
        }

        public override NameValueCollection Headers {
            get { return headers; }
        }

        public string Body
        {
            get {
                var reader = new StreamReader(OutputStream);
                outputStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public override void AddHeader(string name, string value){
            Headers.Add(name, value);
        }

        public override void Write(string s){
            Console.WriteLine(s);
        }

    }
}