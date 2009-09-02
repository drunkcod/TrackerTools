namespace TrackerTools
open System
open System.IO
open System.Net
open System.Xml.Serialization

[<AutoOpen>]
module Util = 
    let FromXml<'a> (stream:Stream) =
        use reader = new StreamReader(stream)
        let serializer = XmlSerializer(typeof<'a>)
        serializer.Deserialize(reader) :?> 'a

type Tracker =
    val baseUrl : string
    val token : string
    
    new(token) = { baseUrl = "http://www.pivotaltracker.com/services/v2/"; token = token }
    new(token, baseUrl) = { token = token; baseUrl = baseUrl }
    
    inherit Service
        override this.BaseUrl = this.baseUrl
        override this.PrepareRequest request = request.Headers.Add("X-TrackerToken", this.token)
        
    member this.CreateProject project = this.Post "projects" (XmlRequest(project))
        
    member this.GetProjects() = this.Get("projects") 

    member this.GetStories(projectId:int) = this.Get(String.Format("projects/{0}/stories", projectId))
    member this.GetStory projectId storyId = this.Get(String.Format("projects/{0}/stories/{1}", projectId, storyId))
    
    member this.GetTasks projectId storyId = this.Get(String.Format("projects/{0}/stories/{1}/tasks", projectId, storyId))
    member this.AddTask (projectId:int) (storyId:int) = this.Post(String.Format("projects/{0}/stories/{1}/tasks", projectId, storyId)) : #IRequestHandler -> #IResponseHandler -> unit

    member this.GetIteration projectId iteration = this.Get(String.Format("projects/{0}/iterations/{1}", projectId, iteration))

type TrackerApi(token) =
    let tracker = Tracker(token)
    member this.Base = tracker
    member this.GetProjects() = tracker.GetProjects() FromXml<TrackerProjects>
    member this.GetIteration(projectId:int, iteration) = 
        let result = tracker.GetIteration projectId iteration FromXml<TrackerIterations>
        result.Items
        
    member this.GetStories(projectId:int) = tracker.GetStories(projectId) FromXml<TrackerStories>
    member this.GetStory(projectId, storyId) = tracker.GetStory projectId storyId FromXml<TrackerStory>
    member this.GetTasks(projectId:int, storyId:int) = tracker.GetTasks projectId storyId FromXml<TrackerTasks>