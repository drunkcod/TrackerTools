using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace TrackerTools.Web
{
    public class BasicHttpRequest : HttpRequestBase
    {
        readonly Uri url;
        readonly string httpMethod;
        readonly NameValueCollection form = new NameValueCollection();
        readonly NameValueCollection queryString = new NameValueCollection();
        readonly NameValueCollection headers = new NameValueCollection();
        readonly Stream inputStream;

        BasicHttpRequest(Uri url, string httpMethod):this(url, httpMethod, Stream.Null){}

        BasicHttpRequest(Uri url, string httpMethod, Stream inputStream) {
            this.url = url;
            this.httpMethod = httpMethod;
            this.inputStream = inputStream;
        }

        public static BasicHttpRequest Get(string url){ return new BasicHttpRequest(new Uri(url), "GET"); }
        public static BasicHttpRequest Put(string url){ return new BasicHttpRequest(new Uri(url), "PUT"); }
        public static BasicHttpRequest Post(string url) { return new BasicHttpRequest(new Uri(url), "POST"); }

        public override string AppRelativeCurrentExecutionFilePath {
            get { return "~" + RawUrl; }
        }

        public override string PathInfo {
            get { return string.Empty; }
        }

        public override string Path {
            get { return url.AbsolutePath; }
        }

        public override NameValueCollection Form { get { return form; } }
        public override NameValueCollection QueryString { get { return queryString; } }
        public override string RawUrl { get { return url.AbsolutePath; } }
        public override string HttpMethod { get { return httpMethod; } }
        public override NameValueCollection Headers { get { return headers; } }
        public override Stream InputStream { get { return inputStream; } }
        public override void ValidateInput(){ }
    }
}