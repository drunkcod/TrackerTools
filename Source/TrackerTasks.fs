namespace TrackerTools
open System.Collections.Generic
open System.Xml
open System.Xml.Serialization

[<XmlRoot("tasks")>]
type TrackerTasks() =
    let tasks = List<TrackerTask>()
    [<XmlElement("task")>]
    member this.Tasks = tasks