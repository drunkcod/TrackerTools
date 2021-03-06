﻿namespace TrackerTools
open System.Collections.Generic
open System.Xml
open System.Xml.Serialization

[<XmlRoot("stories")>]
type TrackerStories() =
    [<DefaultValue>] val mutable private total : int
    let stories = List<TrackerStory>()
    [<XmlElement("story")>]
    member this.Items = stories
    member this.Count = stories.Count
    [<XmlAttribute("total")>]
    member this.Total
        with get() = this.total
        and set(value) = this.total <- value