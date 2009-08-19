namespace TrackerTools
open System.Xml.Serialization
open System.Collections.Generic

[<XmlRoot("projects")>]
type TrackerProjects() =
    let projects = List<TrackerProject>()
    [<XmlElement("project")>]
    member this.Projects = projects
    
