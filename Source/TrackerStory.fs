namespace TrackerTools
open System.Xml.Serialization

[<XmlRoot("story")>]
type TrackerStory() =
    [<DefaultValue;XmlElement("id")>] val mutable Id : int
    [<DefaultValue;XmlElement("story_type")>] val mutable StoryType : string
    [<DefaultValue;XmlElement("url")>] val mutable Url : string
    [<DefaultValue;XmlElement("estimate")>] val mutable Estimate : int
    [<DefaultValue;XmlElement("current_state")>] val mutable CurrentState : string
    [<DefaultValue;XmlElement("description")>] val mutable Description : string
    [<DefaultValue;XmlElement("name")>] val mutable Name : string
    [<DefaultValue;XmlElement("requested_by")>] val mutable RequestedBy : string
    [<DefaultValue;XmlElement("owned_by")>] val mutable OwnedBy : string
    [<DefaultValue;XmlElement("created_at")>] val mutable CreatedAt : string
    [<DefaultValue;XmlElement("accepted_at")>] val mutable AcceptedAt : string
    [<DefaultValue;XmlElement("labels")>] val mutable Labels : string