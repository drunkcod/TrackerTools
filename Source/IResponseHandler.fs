namespace TrackerTools
open System.Net

type IResponseHandler =
    abstract HandleResponse : HttpWebResponse -> unit