using System;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Web.Mvc;

namespace TrackerTools.Web
{
    public class XmlResult : ActionResult
    {
        static readonly XmlWriterSettings DefaultXmlWritterSettings = new XmlWriterSettings()
        {
            Encoding = Encoding.UTF8,
            Indent = true
        };

        readonly object data;

        public XmlResult(object data)
        {
            this.data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var serializer = new XmlSerializer(data.GetType());
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var response = context.HttpContext.Response;
            response.ContentType = "text/xml";
            using (var writer = XmlWriter.Create(response.OutputStream, DefaultXmlWritterSettings))
                serializer.Serialize(writer, data, ns);
        }
    }
}
