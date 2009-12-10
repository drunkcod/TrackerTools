using System;
using System.Web;

namespace TrackerTools.Web
{
    class BasicContext : HttpContextBase
    {
        readonly HttpRequestBase request;
        readonly HttpResponseBase response;
        readonly HttpSessionStateBase session = new MemoryHttpSessionState();

        public BasicContext(HttpRequestBase request, HttpResponseBase response){
            this.request = request;
            this.response = response;
        }

        public override HttpRequestBase Request { get { return request; } }
        public override HttpResponseBase Response { get { return response; } }

        public override HttpSessionStateBase Session { get { return session; } }

        public HttpContext ToHttpContext() {

            return new HttpContext(new HttpContextWorkerAdapter(this));
        }
    }
}