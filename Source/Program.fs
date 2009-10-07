namespace TrackerTools
open System
open System.IO
open System.Net
open System.Xml
open System.Xml.Serialization
open System.Text
open System.Reflection

type ConsoleApplication(configuration:TrackerToolsConfiguration) =
    let tracker = TrackerApi(configuration.ApiToken)

    member private this.Bind ifMissing (argument:ParameterInfo) = 
        match argument.ParameterType with
        | x when x = typeof<TrackerApi> -> box tracker
        | x when x = typeof<TrackerToolsConfiguration> -> box configuration
        | _ -> ifMissing(argument)

    member this.FindCommand bind name =
        let command =  
            Assembly.GetExecutingAssembly().GetTypes()
            |> Seq.filter (fun x -> typeof<ITrackerToolsCommand>.IsAssignableFrom(x))
            |> Seq.tryFind (fun x -> 
                let commandNames = x.GetCustomAttributes(typeof<CommandNameAttribute>, false)
                (commandNames.[0] :?> CommandNameAttribute).Name = name)
        let bind = this.Bind bind                
        command |> Option.map (fun x -> 
            let ctor = x.GetConstructors().[0]
            let parameters = ctor.GetParameters()
            let args : obj array = Array.zeroCreate parameters.Length
            parameters |> Seq.iteri (fun n x -> args.[n] <- bind x)
            ctor.Invoke(args) :?> ITrackerToolsCommand)
            
module Program =                             
    let ShowHelp() = ()
    
    let readConfiguration = function
        | "" -> TrackerToolsConfiguration.FromAppConfig()
        | from -> TrackerToolsConfiguration.From(from + ".xml")
    
    let [<EntryPoint>] main args =
        try
            let commandName = args.[0]
            let commandLineBinder = CommandLineParameterBinder(args)            
            let application = ConsoleApplication(readConfiguration(commandLineBinder.GetOption("project")))
            match application.FindCommand (commandLineBinder.Bind) commandName with
            | Some(command) -> command.Invoke()
            | None -> ShowHelp()
        with :? WebException as e ->
            Console.WriteLine e.Message       
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine                    
        0