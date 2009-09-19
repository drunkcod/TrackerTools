namespace TrackerTools
open System
open System.IO
open System.Net
open System.Xml
open System.Xml.Serialization
open System.Text
open System.Reflection

module Program =
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
                                         
    let ShowHelp() = ()

    [<EntryPoint>]
    let main args =
        try
            let commandName = args.[0]
            let commandLineBinder = CommandLineParameterBinder(args)
            match FindCommand (Bind commandLineBinder.Bind) commandName with
            | Some(command) -> command.Invoke()
            | None -> ShowHelp()
        with :? WebException as e ->
            Console.WriteLine e.Message       
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine                    
        0