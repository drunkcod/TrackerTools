namespace TrackerTools
open System
open System.Reflection
  
type CommandLineParameterBinder(args:string array) =
    let OptionPrefix = "--"  
    
    let freeArgs = 
        let isFreeArg (x:string) = not (x.StartsWith(OptionPrefix))
        args |> Seq.filter isFreeArg |> Seq.to_array

    member this.Bind (parameter:ParameterInfo) = 
        let fromCommandLine = parameter.GetCustomAttributes(typeof<FromCommandLineAttribute>, false)
        Convert.ChangeType(freeArgs.[(fromCommandLine.[0] :?> FromCommandLineAttribute).Position], parameter.ParameterType)

    member this.GetOption name =
        match args |> Seq.tryFind (fun x -> x.StartsWith(OptionPrefix + name)) with
        | None -> ""
        | Some(value) -> value.Split('=').[1]