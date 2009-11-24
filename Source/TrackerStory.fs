namespace TrackerTools
open System
open System.IO
open System.Xml.Serialization

[<XmlRoot("story")>]
type TrackerStory() =
    [<DefaultValue>] val mutable private id : int
    [<DefaultValue>] val mutable private name : string
    [<DefaultValue>] val mutable private storyType : string
    [<DefaultValue>] val mutable private url : string
    [<DefaultValue>] val mutable private estimate : int
    [<DefaultValue>] val mutable private currentState : string
    [<DefaultValue>] val mutable private description : string
    [<DefaultValue>] val mutable private requestedBy : string
    [<DefaultValue>] val mutable private ownedBy : string
    [<DefaultValue>] val mutable private createdAt : string
    [<DefaultValue>] val mutable private acceptedAt : string
    [<DefaultValue>] val mutable private labels : string

    [<XmlElement("id")>] member this.Id with get() = this.id and set(value) = this.id <- value
    [<XmlElement("story_type")>] member this.StoryType with get() = this.storyType and set(value) = this.storyType <- value
    [<XmlElement("url")>] member this.Url with get() = this.url and set(value) = this.url <- value
    [<XmlElement("estimate")>] member this.Estimate with get() = this.estimate and set(value) = this.estimate <- value
    [<XmlElement("current_state")>] member this.CurrentState with get() = this.currentState and set(value) = this.currentState <- value
    [<XmlElement("description")>] member this.Description with get() = this.description and set(value) = this.description <- value
    [<XmlElement("name")>] member this.Name with get() = this.name and set(value) = this.name <- value
    [<XmlElement("requested_by")>] member this.RequestedBy with get() = this.requestedBy and set(value) = this.requestedBy <- value
    [<XmlElement("owned_by")>] member this.OwnedBy with get() = this.ownedBy and set(value) = this.ownedBy <- value
    [<XmlElement("created_at")>] member this.CreatedAt with get() = this.createdAt and set(value) = this.createdAt <- value
    [<XmlElement("accepted_at")>] member this.AcceptedAt with get() = this.acceptedAt and set(value) = this.acceptedAt <- value
    [<XmlElement("labels")>] member this.Labels with get() = this.labels and set(value) = this.labels <- value
    
    
    member this.WriteStoryCard template =
            let ProcessTemplate (story:TrackerStory) =
                let transform (sym:Symbol) = 
                    if sym.IsKnownAs "Description" then
                        sym.ToString().Replace("\n","<br>")
                    else sym.ToString()                                   
                DataBinder.Bind(template,  story, transform)
            File.WriteAllText(String.Format("{0}.html", this.Id), ProcessTemplate this)