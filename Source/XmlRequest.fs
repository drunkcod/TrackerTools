namespace TrackerTools
open System.IO
open System.Net
open System.Text
open System.Xml
open System.Xml.Serialization

type XmlRequest(value:obj) =
    member this.WriteFragment(stream:Stream, closeOutput) =
            use writer =
                XmlTextWriter.Create(stream,
                    XmlWriterSettings(
                        OmitXmlDeclaration = true,
                        Encoding = Encoding.UTF8,
                        CloseOutput = closeOutput))
            let serializer = XmlSerializer(value.GetType())
            let ns = XmlSerializerNamespaces()
            ns.Add("", "")
            serializer.Serialize(writer, value, ns)

    member this.WriteFragment(stream) = this.WriteFragment(stream, true)
    member this.ToStream() =
        let stream = new MemoryStream()
        this.WriteFragment(stream, false)
        stream.Position <- 0L
        stream
        
    interface IRequestHandler with
        member this.HandleRequest(request:HttpWebRequest) =
            request.ContentType <- "text/xml"
            this.WriteFragment (request.GetRequestStream())