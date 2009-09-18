namespace TrackerTools
open System
open System.IO
open System.Net
open System.Xml
open System.Xml.Serialization
open System.Text
open System.Reflection

module Program =                   
    let StoryTemplate() = File.ReadAllText("StoryTemplate.html")
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    let Tracker = TrackerApi(Configuration.ApiToken)

    let Bind ifMissing (argument:ParameterInfo) = 
        match argument.ParameterType with
        | x when x = typeof<TrackerApi> -> box Tracker
        | x when x = typeof<TrackerToolsConfiguration> -> box Configuration
        | _ -> ifMissing(argument)

    let FindCommand bind name =
        let command =  
            Assembly.GetExecutingAssembly().GetTypes()
            |> Seq.filter (fun x -> typeof<ITrackerToolsCommand>.IsAssignableFrom(x))
            |> Seq.tryFind (fun x -> 
                let commandNames = x.GetCustomAttributes(typeof<CommandNameAttribute>, false)
                (commandNames.[0] :?> CommandNameAttribute).Name = name)
        command |> Option.map (fun x -> 
            let ctor = x.GetConstructors().[0]
            let parameters = ctor.GetParameters()
            let args : obj array = Array.zeroCreate parameters.Length
            parameters |> Seq.iteri (fun n x -> args.[n] <- bind x)
            ctor.Invoke(args) :?> ITrackerToolsCommand)
   
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
           
    let ResponseHandler withResponse = {new IResponseHandler with member this.HandleResponse x = withResponse(x)}           
            
    let AddTask (storyId:int) (description:string) =
        let request = XmlRequest(TrackerTask(Description = description)) 
        let response = (ResponseHandler(fun x -> 
            use stream = x.GetResponseStream()
            DumpToConsole stream))
        Tracker.Base.AddTask Configuration.ProjectId storyId request response
        
    let ShowHelp() = ()

    [<EntryPoint>]
    let main args =
        try
            let commandName = args.[0]
            let commandLineBinder = CommandLineParameterBinder(args)
            match FindCommand (Bind commandLineBinder.Bind) commandName with
            | Some(command) -> command.Invoke()
            | None ->
                match commandName with
                | "CreateStoryCard" -> CreateStoryCard (Int32.Parse(args.[1]))
                | "AddTask" -> AddTask (Int32.Parse(args.[1])) (args.[2])
                | _ -> ShowHelp()
        with :? WebException as e ->
            Console.WriteLine e.Message       
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine                    
        0