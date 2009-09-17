namespace TrackerTools
open System
open System.Reflection
open NUnit.Framework

[<AttributeUsage(AttributeTargets.Parameter)>]
type FromCommandLineAttribute() =
    inherit Attribute()
    let mutable position = -1
    member this.Position with get() = position and set value = position <- value
    
type CommandLineParameterBinder(args:string array) =
    member this.Bind (parameter:ParameterInfo) = 
        let fromCommandLine = parameter.GetCustomAttributes(typeof<FromCommandLineAttribute>, false)
        Convert.ChangeType(args.[(fromCommandLine.[0] :?> FromCommandLineAttribute).Position], parameter.ParameterType)

type SampleType() =
    member this.Method1([<FromCommandLine(Position = 1)>] x:int) = ()

module CommandLineParameterBinderTests =
    let fromArg1 = typeof<SampleType>.GetMethod("Method1").GetParameters().[0]

    let [<Test>] should_support_positional_binding() =
        let binder = CommandLineParameterBinder([|"1"; "2"; "3"|])
        let bound = binder.Bind(fromArg1)
        Assert.That(bound, Is.EqualTo(2))