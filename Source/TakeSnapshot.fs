namespace TrackerTools
open System
open System.Collections.Generic
open System.IO
open System.Xml
open System.Xml.Serialization

type TrackerStory() =
    [<DefaultValue>] val mutable id : int
    [<DefaultValue>] val mutable story_type : string
    [<DefaultValue>] val mutable url : string
    [<DefaultValue>] val mutable estimate : int
    [<DefaultValue>] val mutable current_state : string
    [<DefaultValue>] val mutable description : string
    [<DefaultValue>] val mutable name : string
    [<DefaultValue>] val mutable requested_by : string
    [<DefaultValue>] val mutable owned_by : string
    [<DefaultValue>] val mutable created_at : string
    [<DefaultValue>] val mutable accepted_at : string
    [<DefaultValue>] val mutable labels : string
                    
[<XmlRoot("stories")>]
type TrackerStories() =
    [<DefaultValue>] val mutable private total : int
    let stories = List<TrackerStory>()
    [<XmlElement("story")>]        
    member this.Stories = stories
    member this.Count = stories.Count
    member this.Total
        with get() = this.total
        and set(value) = this.total <- value

module Program =   
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    
    let SaveSnapshot (stream:Stream) = 
        use reader = new StreamReader(stream)
        let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
        File.WriteAllText(targetPath, reader.ReadToEnd())
       
    [<EntryPoint>]
    let main array =
        let tracker = Tracker(Configuration.ApiToken)
        tracker.GetStories Configuration.ProjectId SaveSnapshot
        0