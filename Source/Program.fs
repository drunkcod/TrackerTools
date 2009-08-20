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

    let TakeSnapshot (tracker:Tracker) =
        let SaveSnapshot (stream:Stream) = 
            use reader = new StreamReader(stream)
            let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
            File.WriteAllText(targetPath, reader.ReadToEnd())
        tracker.GetStories Configuration.ProjectId SaveSnapshot
    
    let WriteStoryCard(story:TrackerStory) =
        let FormatDescription (story:TrackerStory) = 
            match story.Description with
            | null -> String.Empty
            | x -> x.Replace("\n", "<br>")
            
        let ProcessTemplate (story:TrackerStory) =
            StoryTemplate.Replace("$(Name)", story.Name).Replace("$(Description)",FormatDescription story )
        File.WriteAllText(String.Format("{0}.html", story.Id), ProcessTemplate story)
    
    let CreateStoryCard (storyId:int) (tracker:Tracker) =
        tracker.GetStory Configuration.ProjectId storyId FromXml<TrackerStory>
        |> WriteStoryCard

    let DumpToConsole (stream:Stream) =
        use reader = new StreamReader(stream)
        reader.ReadToEnd() |> Console.WriteLine

    let ShowTasks (storyId:int) (tracker:Tracker) =
        tracker.GetTasks Configuration.ProjectId storyId DumpToConsole
            
    let AddTask (storyId:int) (description:string) (tracker:Tracker) =
        let request = XmlRequest(TrackerTask(Description = description)) 
        tracker.AddTask Configuration.ProjectId storyId request DumpToConsole
        
    let DumpCurrentIteration (tracker:Tracker) = 
        let iterations = tracker.GetIteration Configuration.ProjectId "current" FromXml<TrackerIterations>
        
        iterations.Items
        |> Seq.collect (fun x -> x.Stories)
        |> Seq.iter WriteStoryCard
    
    let ShowHelp x = ()

    [<EntryPoint>]
    let main args =
        try
            Tracker(Configuration.ApiToken)
            |> match args.[0] with
                | "TakeSnaphot" -> TakeSnapshot
                | "CreateStoryCard" -> CreateStoryCard (Int32.Parse(args.[1]))
                | "ShowTasks" -> ShowTasks (Int32.Parse(args.[1]))
                | "AddTask" -> AddTask (Int32.Parse(args.[1])) (args.[2])
                | "DumpCurrentIteration" -> DumpCurrentIteration
                | _ -> ShowHelp
        with :? WebException as e ->
            Console.WriteLine e.Message       
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine                    
        0