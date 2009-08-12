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
    
    let CreateStoryCard (storyId:int) (tracker:Tracker) =
        let FormatDescription (story:TrackerStory) = story.Description.Replace("\n", "<br>")
        let ProcessTemplate (story:TrackerStory) =
            StoryTemplate.Replace("$(Name)", story.Name).Replace("$(Description)",FormatDescription story )
        let story = tracker.GetStory Configuration.ProjectId storyId FromXml<TrackerStory>
        File.WriteAllText(String.Format("{0}.html", storyId), ProcessTemplate story)
    
    let ShowHelp x = ()

    [<EntryPoint>]
    let main args =
        Tracker(Configuration.ApiToken)
        |> match args.[0] with
            | "TakeSnaphot" -> TakeSnapshot
            | "CreateStoryCard" -> CreateStoryCard (Int32.Parse(args.[1]))
            | _ -> ShowHelp
        0