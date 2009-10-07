namespace TrackerTools
open System
open System.IO
open System.Configuration
open System.Xml
open System.Xml.Serialization

[<Sealed;XmlRoot("Tracker")>]
type TrackerToolsConfiguration() =
    
    [<DefaultValue>] val mutable private apiToken : string
    [<DefaultValue>] val mutable private projectId : int
    [<DefaultValue>] val mutable private outputDirectory : string
    [<DefaultValue>] val mutable private storyTemplate : StoryTemplateItem

    static member SectionName = "Tracker"

    static member FromAppConfig() =
        let section = ConfigurationManager.GetSection(TrackerToolsConfiguration.SectionName) 
        if section = null then
            raise(ArgumentException(TrackerToolsConfiguration.SectionName + " section not found in configuration file."))
        section :?> TrackerToolsConfiguration
        
    static member From(path) = 
        use file = File.OpenText(path)
        let serializer = XmlSerializer(typeof<TrackerToolsConfiguration>)
        use reader = new XmlTextReader(file)
        serializer.Deserialize(reader) :?> TrackerToolsConfiguration))

    member this.ApiToken
        with get() = this.apiToken
        and set(value) = this.apiToken <- value

    member this.ProjectId
        with get() = this.projectId
        and set(value) = this.projectId <- value
        
    member this.OutputDirectory 
        with get() = this.outputDirectory
        and set(value) = this.outputDirectory <- value        

    member this.StoryTemplate
        with get() = this.storyTemplate
        and set(value) = this.storyTemplate <- value

    interface IConfigurationSectionHandler with
        member this.Create(parent, configcontext, section) =
            let serializer = XmlSerializer(this.GetType())                
            use reader = new XmlNodeReader(section) :> XmlReader
            serializer.Deserialize(reader)                                 
