namespace TrackerTools
open System.Collections.Generic
open System.Xml
open System.Xml.Serialization

[<XmlRoot("iterations")>]
type TrackerIterations() =
    let iterations = List<TrackerIteration>()
    
    [<XmlElement("iteration")>]
    member this.Items = iterations