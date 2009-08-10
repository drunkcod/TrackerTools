namespace TrackerTools

open System
open System.Net
open System.IO
open System.Xml
open System.Collections.Generic
open System.Xml.Serialization
open System.Configuration

module Util =
    let fromXml<'a> (stream:Stream) =
        let serializer = XmlSerializer(typeof<'a>)
        serializer.Deserialize(stream) :?> 'a

type Tracker(token) =
    member this.Get(url:string, responseHandler) =
        let request = WebRequest.Create(url)
        request.Headers.Add("X-TrackerToken", token)
        use response = request.GetResponse()
        responseHandler(response.GetResponseStream())

    member this.Get<'a> (url:string) =
        this.Get(url, Util.fromXml<'a>)
        
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

[<Sealed;XmlRoot("Tracker")>]
type TrackerToolsConfiguration() =   
    [<DefaultValue>] val mutable private apiToken : string
    [<DefaultValue>] val mutable private projectId : int
    [<DefaultValue>] val mutable private outputDirectory : string

    member this.ApiToken
        with get() = this.apiToken
        and set(value) = this.apiToken <- value

    member this.ProjectId
        with get() = this.projectId
        and set(value) = this.projectId <- value
        
    member this.OutputDirectory 
        with get() = this.outputDirectory
        and set(value) = this.outputDirectory <- value        

    interface IConfigurationSectionHandler with
        member this.Create(parent, configcontext, section) =
            let serializer = XmlSerializer(this.GetType())                
            use reader = new XmlNodeReader(section) :> XmlReader
            serializer.Deserialize(reader)                                 

module Program =   
    let Configuration = ConfigurationManager.GetSection("Tracker") :?> TrackerToolsConfiguration 
    
    let SaveSnapshot (stream:Stream) = 
        use reader = new StreamReader(stream)
        let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
        File.WriteAllText(targetPath, reader.ReadToEnd())
       
    [<EntryPoint>]
    let main array =
        let tracker = Tracker(Configuration.ApiToken)
        tracker.Get(String.Format("http://www.pivotaltracker.com/services/v2/projects/{0}/stories", Configuration.ProjectId), SaveSnapshot)       
        0
