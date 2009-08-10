namespace TrackerTools

open System
open System.Net
open System.IO
open System.Xml
open System.Collections.Generic
open System.Xml.Serialization
open System.Configuration

type Tracker(token) =
    static let ApiUrl = "http://www.pivotaltracker.com/services/v2"

    member this.Get (url:string) responseHandler =
        let request = WebRequest.Create(url)
        request.Headers.Add("X-TrackerToken", token)
        use response = request.GetResponse()
        responseHandler(response.GetResponseStream())
        
    member this.GetStories projectId = this.Get(String.Format("{0}/projects/{1}/stories", ApiUrl, projectId))
        
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