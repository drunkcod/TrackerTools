namespace TrackerTools
open System
open System.Net

type Tracker(token) =
    static let ApiUrl = "http://www.pivotaltracker.com/services/v2"

    member this.Get (url:string) responseHandler =
        let request = WebRequest.Create(url)
        request.Headers.Add("X-TrackerToken", token)
        use response = request.GetResponse()
        responseHandler(response.GetResponseStream())

    member this.Post (url:string) (requestHandler:#IRequestHandler) responseHandler =
        let request = WebRequest.Create(url) :?> HttpWebRequest
        request.Headers.Add("X-TrackerToken", token)
        request.Method <- "POST"
        requestHandler.HandleRequest(request)
        use response = request.GetResponse()
        responseHandler(response.GetResponseStream())

    member this.GetStories projectId = this.Get(String.Format("{0}/projects/{1}/stories", ApiUrl, projectId))
    member this.GetStory projectId storyId = this.Get(String.Format("{0}/projects/{1}/stories/{2}", ApiUrl, projectId, storyId))
    
    member this.GetTasks projectId storyId = this.Get(String.Format("{0}/projects/{1}/stories/{2}/tasks", ApiUrl, projectId, storyId))
    member this.AddTask projectId storyId =this.Post(String.Format("{0}/projects/{1}/stories/{2}/tasks", ApiUrl, projectId, storyId))