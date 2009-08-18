namespace TrackerTools
open System.Xml.Serialization
type IgnoreIf = System.ComponentModel.DefaultValueAttribute

[<XmlRoot("task")>]
type TrackerTask() =
    [<DefaultValue;XmlElement("id");IgnoreIf(0)>] val mutable Id : int
    [<DefaultValue;XmlElement("description")>] val mutable Description : string
    [<DefaultValue;XmlElement("position");IgnoreIf(0)>] val mutable Position : int
    [<DefaultValue;XmlElement("complete");IgnoreIf(false)>] val mutable Complete : bool
    [<DefaultValue;XmlElement("created_at")>] val mutable CreatedAt : string