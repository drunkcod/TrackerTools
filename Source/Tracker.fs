namespace TrackerTools
open System

open System.Xml.Serialization
open System.IO
[<AutoOpen>]
module Util = 
    let FromXml<'a> (stream:Stream) =
        use reader = new StreamReader(stream)
        let serializer = XmlSerializer(typeof<'a>)
        serializer.Deserialize(reader) :?> 'a

type Tracker(token) =
    inherit Service()
        override this.BaseUrl = "http://www.pivotaltracker.com/services/v2/"
        override this.PrepareRequest request = request.Headers.Add("X-TrackerToken", token)
        
    member this.GetProjects() = this.Get("projects") 

    member this.GetStories(projectId:int) = this.Get(String.Format("projects/{0}/stories", projectId))
    member this.GetStory projectId storyId = this.Get(String.Format("projects/{0}/stories/{1}", projectId, storyId))
    
    member this.GetTasks projectId storyId = this.Get(String.Format("projects/{0}/stories/{1}/tasks", projectId, storyId))
    member this.AddTask (projectId:int) (storyId:int) = this.Post(String.Format("projects/{0}/stories/{1}/tasks", projectId, storyId))

    member this.GetIteration projectId iteration = this.Get(String.Format("projects/{0}/iterations/{1}", projectId, iteration))

type TrackerApi(token) =
    let tracker = Tracker(token)
    
    member this.GetProjects() = tracker.GetProjects() FromXml<TrackerProjects>
    member this.GetStories(projectId:int) = tracker.GetStories(projectId) FromXml<TrackerStories>
    member this.GetTasks(projectId:int, storyId:int) = tracker.GetTasks projectId storyId FromXml<TrackerTasks>