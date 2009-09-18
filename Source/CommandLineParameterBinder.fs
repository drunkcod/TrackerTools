namespace TrackerTools
open System
open System.Reflection
  
type CommandLineParameterBinder(args:string array) =
    member this.Bind (parameter:ParameterInfo) = 
        let fromCommandLine = parameter.GetCustomAttributes(typeof<FromCommandLineAttribute>, false)
        Convert.ChangeType(args.[(fromCommandLine.[0] :?> FromCommandLineAttribute).Position], parameter.ParameterType)
