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
        
    member private this.Commands =         
            typeof<ITrackerToolsCommand>.Assembly.GetTypes()
            |> Seq.filter (fun x -> typeof<ITrackerToolsCommand>.IsAssignableFrom(x))
            |> Seq.choose (fun x ->
                let names = x.GetCustomAttributes(typeof<CommandNameAttribute>, false)
                if names.Length > 0 then
                    Some((names.[0] :?> CommandNameAttribute).Name, x)
                else None)                                        
        
    member this.FindCommand bind name =
        let command = this.Commands |> Seq.tryFind (fun (commandName, _) -> commandName = name)
        let bind = this.Bind bind
        command |> Option.map (fun (_,x) ->
            let ctor = x.GetConstructors().[0]
            let parameters = ctor.GetParameters()
            let args : obj array = Array.zeroCreate parameters.Length
            parameters |> Seq.iteri (fun n x -> args.[n] <- bind x)
            ctor.Invoke(args) :?> ITrackerToolsCommand)

    member this.ShowHelp (output:TextWriter) =
        output.WriteLine "usage is:TrackerTools <command>"
        this.Commands |> Seq.iter (fun (name, x) -> output.WriteLine(name))
    
module Program =
    let readConfiguration = function
        | "" -> TrackerToolsConfiguration.FromAppConfig()
        | from -> TrackerToolsConfiguration.From(from + ".xml")

    let [<EntryPoint>] main args =       
        try
            let commandLineBinder = CommandLineParameterBinder(args)
            let commandName = commandLineBinder.Bind<string>(0)
            let application = ConsoleApplication(readConfiguration(commandLineBinder.GetOption("project")))
            match application.FindCommand (commandLineBinder.Bind) commandName with
            | Some(command) -> command.Invoke()
            | None -> application.ShowHelp Console.Out
        with :? WebException as e ->
            Console.WriteLine e.Message
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine
        0