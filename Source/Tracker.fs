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

    member this.GetStories projectId = this.Get(String.Format("{0}/projects/{1}/stories", ApiUrl, projectId))