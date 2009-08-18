namespace TrackerTools
open System.IO
open System.Net
open System.Text
open System.Xml
open System.Xml.Serialization

type XmlRequest(value:obj) =
    member this.WriteFragment(stream:Stream) =
            use writer = 
                XmlTextWriter.Create(stream, 
                    XmlWriterSettings(
                        OmitXmlDeclaration = true, 
                        Encoding = Encoding.UTF8, 
                        CloseOutput = true))
            let serializer = XmlSerializer(value.GetType())
            let ns = XmlSerializerNamespaces()
            ns.Add("", "")
            serializer.Serialize(writer, value, ns)
            
    interface IRequestHandler with
        member this.HandleRequest(request:HttpWebRequest) =
            request.ContentType <- "text/xml"
            this.WriteFragment (request.GetRequestStream())