namespace TrackerTools
open System
open System.IO
open System.Xml.Serialization
 
module Program =
    let StoryTemplate = File.ReadAllText("StoryTemplate.html")
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    let FromXml<'a> (stream:Stream) =
        use reader = new StreamReader(stream)
        let serializer = XmlSerializer(typeof<'a>)
        serializer.Deserialize(reader) :?> 'a

    let TakeSnapshot (tracker:Tracker) =
        let SaveSnapshot (stream:Stream) = 
            use reader = new StreamReader(stream)
            let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
            File.WriteAllText(targetPath, reader.ReadToEnd())
        tracker.GetStories Configuration.ProjectId SaveSnapshot
    
    let CreateStoryCard storyId (tracker:Tracker) =
        let story = tracker.GetStory Configuration.ProjectId storyId FromXml<TrackerStory>
        File.WriteAllText("Output.html", StoryTemplate.Replace("$(Name)", story.Name).Replace("$(Description)", story.Description.Replace("\n", "<br>")))
    
    let ShowHelp x = ()

    [<EntryPoint>]
    let main args =
        Tracker(Configuration.ApiToken)
        |> match args.[0] with
        | "TakeSnaphot" -> TakeSnapshot
        | "CreateStoryCard" -> CreateStoryCard (Int32.Parse(args.[1]))
        | _ -> ShowHelp
        0