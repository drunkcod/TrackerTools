namespace TrackerTools
open System.Collections.Generic
open System.Xml.Serialization

[<XmlRoot("iteration")>]
type TrackerIteration() =
    let stories = List<TrackerStory>()

    [<DefaultValue;XmlElement("id")>] val mutable Id : int
    [<DefaultValue;XmlElement("number")>] val mutable Number : string
    [<DefaultValue;XmlElement("start")>] val mutable StartDate: string
    [<DefaultValue;XmlElement("finish");>] val mutable StopDate: string
    [<XmlArray("stories"); XmlArrayItem("story")>] member this.Stories = stories