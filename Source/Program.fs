namespace TrackerTools
open System
open System.IO
open System.Net
open System.Xml
open System.Xml.Serialization
open System.Text

module Program =
    let StoryTemplate = File.ReadAllText("StoryTemplate.html")
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    let Tracker = TrackerApi(Configuration.ApiToken)

    let TakeSnapshot() =
        let SaveSnapshot (stream:Stream) = 
            use reader = new StreamReader(stream)
            let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
            File.WriteAllText(targetPath, reader.ReadToEnd())
        Tracker.Base.GetStories Configuration.ProjectId SaveSnapshot
    
    let WriteStoryCard(story:TrackerStory) =
        let ProcessTemplate (story:TrackerStory) =
            DataBinder.Bind(StoryTemplate, story).Replace("\n", "<br>")

        File.WriteAllText(String.Format("{0}.html", story.Id), ProcessTemplate story)
    
    let CreateStoryCard (storyId:int) =
        Tracker.GetStory(Configuration.ProjectId, storyId)
        |> WriteStoryCard

    let DumpToConsole (stream:Stream) =
        use reader = new StreamReader(stream)
        reader.ReadToEnd() |> Console.WriteLine

    let ShowTasks (storyId:int) =
        Tracker.Base.GetTasks Configuration.ProjectId storyId DumpToConsole
            
    let AddTask (storyId:int) (description:string) =
        let request = XmlRequest(TrackerTask(Description = description)) 
        Tracker.Base.AddTask Configuration.ProjectId storyId request DumpToConsole
        
    let DumpCurrentIteration() = 
        Tracker.GetIteration(Configuration.ProjectId, "current")        
        |> Seq.collect (fun x -> x.Stories)
        |> Seq.iter WriteStoryCard
    
    let ShowHelp() = ()

    [<EntryPoint>]
    let main args =
        try
            match args.[0] with
            | "TakeSnaphot" -> TakeSnapshot()
            | "CreateStoryCard" -> CreateStoryCard (Int32.Parse(args.[1]))
            | "ShowTasks" -> ShowTasks (Int32.Parse(args.[1]))
            | "AddTask" -> AddTask (Int32.Parse(args.[1])) (args.[2])
            | "DumpCurrentIteration" -> DumpCurrentIteration()
            | _ -> ShowHelp()
        with :? WebException as e ->
            Console.WriteLine e.Message       
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine                    
        0