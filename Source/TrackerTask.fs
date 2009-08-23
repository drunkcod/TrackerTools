namespace TrackerTools
open System.Xml.Serialization
type IgnoreIf = System.ComponentModel.DefaultValueAttribute

[<XmlRoot("task")>]
type TrackerTask() =
    [<DefaultValue>] val mutable private id : int;
    [<DefaultValue>] val mutable private description : string
    [<DefaultValue>] val mutable private position : int
    [<DefaultValue>] val mutable private complete : bool
    [<DefaultValue>] val mutable private createdAt : string
    
    [<XmlElement("id");IgnoreIf(0)>] member this.Id with get() = this.id and set(value) = this.id <- value
    [<XmlElement("description")>] member this.Description with get() = this.description and set(value) = this.description <- value
    [<XmlElement("position");IgnoreIf(0)>] member this.Position with get() = this.position and set(value) = this.position <- value
    [<XmlElement("complete");IgnoreIf(false)>] member this.Complete with get() = this.complete and set(value) = this.complete <- value
    [<XmlElement("created_at")>] member this.CreatedAt with get() = this.createdAt and set(value) = this.createdAt <- value