namespace TrackerTools
open System.Net

type IRequestHandler =
    abstract HandleRequest : HttpWebRequest -> unit