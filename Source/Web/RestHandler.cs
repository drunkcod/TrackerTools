namespace TrackerTools.Web
{
    using System.Web;
    using System.IO;
    
    public class RestHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)        
        {
            using (var writer = new StreamWriter(context.Response.OutputStream))
                writer.WriteLine("Served by {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
            context.Response.OutputStream.Flush();
            context.Response.End();
        }
    }
}
