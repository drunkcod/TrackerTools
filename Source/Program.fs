namespace TrackerTools
open System
open System.IO
open System.Net
open System.Xml
open System.Xml.Serialization
open System.Text

module Program =
    let StoryTemplate() = File.ReadAllText("StoryTemplate.html")
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    let Tracker = TrackerApi(Configuration.ApiToken)

    let SaveSnapshot targetPath (stream:Stream) = 
        use reader = new StreamReader(stream)
        File.WriteAllText(targetPath, reader.ReadToEnd())

    let TakeSnapshot() =
        let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
        Tracker.Base.GetStories Configuration.ProjectId (SaveSnapshot targetPath)
    
    let WriteStoryCard(story:TrackerStory) =
        let ProcessTemplate (story:TrackerStory) =
            DataBinder.Bind(StoryTemplate(), story).Replace("\n", "<br>")

        File.WriteAllText(String.Format("{0}.html", story.Id), ProcessTemplate story)
    
    let CreateStoryCard (storyId:int) =
        Tracker.GetStory(Configuration.ProjectId, storyId)
        |> WriteStoryCard

    let DumpToConsole (stream:Stream) =
        use reader = new StreamReader(stream)
        reader.ReadToEnd() |> Console.WriteLine

    let ShowTasks (storyId:int) =
        Tracker.Base.GetTasks Configuration.ProjectId storyId DumpToConsole
            
    let ResponseHandler withResponse = {new IResponseHandler with member this.HandleResponse x = withResponse(x)}           
            
    let AddTask (storyId:int) (description:string) =
        let request = XmlRequest(TrackerTask(Description = description)) 
        let response = (ResponseHandler(fun x -> 
            use stream = x.GetResponseStream()
            DumpToConsole stream))
        Tracker.Base.AddTask Configuration.ProjectId storyId request response
        
    let DumpCurrentIteration() = 
        Tracker.GetIteration(Configuration.ProjectId, "current")        
        |> Seq.collect (fun x -> x.Stories)
        |> Seq.iter WriteStoryCard

    let CreateBackup() =
        let target = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd"))
        Directory.CreateDirectory(target) |> ignore
        Tracker.GetProjects().Projects
        |> Seq.iter (fun x -> Tracker.Base.GetStories x.Id (SaveSnapshot (Path.Combine(target, x.Name + ".xml"))))
    
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
            | "CreateBackup" -> CreateBackup()
            | _ -> ShowHelp()
        with :? WebException as e ->
            Console.WriteLine e.Message       
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine                    
        0