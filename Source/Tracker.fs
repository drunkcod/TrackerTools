namespace TrackerTools
open System
open System.Net

open System.Xml.Serialization
open System.IO
[<AutoOpen>]
module Util = 
    let FromXml<'a> (stream:Stream) =
        use reader = new StreamReader(stream)
        let serializer = XmlSerializer(typeof<'a>)
        serializer.Deserialize(reader) :?> 'a

type Tracker(token) =
    static let ApiUrl = "http://www.pivotaltracker.com/services/v2"
    
    member this.CreateRequest url = 
        let request = WebRequest.Create(url:string) :?> HttpWebRequest
        request.Headers.Add("X-TrackerToken", token)
        request

    member this.Get (url:string) responseHandler =
        let request = this.CreateRequest url
        use response = request.GetResponse()
        responseHandler(response.GetResponseStream())

    member this.Post (url:string) (requestHandler:#IRequestHandler) responseHandler =
        let request = this.CreateRequest url
        request.Method <- "POST"
        requestHandler.HandleRequest(request)
        use response = request.GetResponse()
        responseHandler(response.GetResponseStream())
        
    member this.GetProjects() = this.Get(String.Format("{0}/projects", ApiUrl)) 

    member this.GetStories projectId = this.Get(String.Format("{0}/projects/{1}/stories", ApiUrl, projectId))
    member this.GetStory projectId storyId = this.Get(String.Format("{0}/projects/{1}/stories/{2}", ApiUrl, projectId, storyId))
    
    member this.GetTasks projectId storyId = this.Get(String.Format("{0}/projects/{1}/stories/{2}/tasks", ApiUrl, projectId, storyId))
    member this.AddTask projectId storyId =this.Post(String.Format("{0}/projects/{1}/stories/{2}/tasks", ApiUrl, projectId, storyId))

    member this.GetIteration projectId iteration = this.Get(String.Format("{0}/projects/{1}/iterations/{2}", ApiUrl, projectId, iteration))


type TrackerApi(token) =
    let tracker = Tracker(token)
    
    member this.GetProjects() = tracker.GetProjects() FromXml<TrackerProjects>
    member this.GetStories(projectId:int) = tracker.GetStories(projectId) FromXml<TrackerStories>
    member this.GetTasks(projectId:int, storyId:int) = tracker.GetTasks projectId storyId FromXml<TrackerTasks>
    