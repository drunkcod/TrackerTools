namespace TrackerTools
open System
open System.IO
open System.Text
open System.Xml
open System.Xml.Serialization

type [<XmlRoot("StoryTemplate")>] StoryTemplateItem() =
    [<DefaultValue>] val mutable private source : string

    [<XmlAttribute>]
    member this.Source
        with get() = this.source
        and set(value) = this.source <- value

    member this.GetTemplate() = File.ReadAllText(this.Source)

    member this.ToXml() =
        let xml = StringBuilder()
        let settings = XmlWriterSettings(OmitXmlDeclaration = true)
        let writer = XmlWriter.Create(xml, settings)
        let serializer = XmlSerializer(this.GetType())
        let ns = XmlSerializerNamespaces()
        ns.Add(String.Empty, String.Empty)
        serializer.Serialize(writer, this, ns)
        xml.ToString()
