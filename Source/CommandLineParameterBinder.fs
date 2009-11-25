namespace TrackerTools
open System
open System.Reflection

type CommandLineParameterBinder(args:string array) =
    let OptionPrefix = "--"

    let freeArgs =
        let isFreeArg (x:string) = not (x.StartsWith(OptionPrefix))
        args |> Seq.filter isFreeArg |> Seq.to_array

    [<OverloadID("BindParameter")>]
    member this.Bind (parameter:ParameterInfo) =
        let fromCommandLine = parameter.GetCustomAttributes(typeof<FromCommandLineAttribute>, false)
        this.Bind((fromCommandLine.[0] :?> FromCommandLineAttribute).Position, parameter.ParameterType)

    [<OverloadID("BindPositional")>]
    member this.Bind(position, wantedType:Type) =
        Convert.ChangeType(freeArgs.[position], wantedType)

    [<OverloadID("BindTyped")>]
    member this.Bind<'a>(position) = this.Bind(position, typeof<'a>) :?> 'a

    member this.GetOption name =
        match args |> Seq.tryFind (fun x -> x.StartsWith(OptionPrefix + name)) with
        | None -> ""
        | Some(value) -> value.Split('=').[1]