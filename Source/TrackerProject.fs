namespace TrackerTools
open System.Xml.Serialization

type TrackerProject() =
    [<DefaultValue>] val mutable private id : int
    [<DefaultValue>] val mutable private name : string
    
    [<XmlElement("id")>] member this.Id with get() = this.id and set(value) = this.id <- value
    [<XmlElement("name")>] member this.Name with get() = this.name and set(value) = this.name <- value