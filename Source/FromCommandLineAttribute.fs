namespace TrackerTools
open System

[<AttributeUsage(AttributeTargets.Parameter)>]
type FromCommandLineAttribute() =
    inherit Attribute()
    let mutable position = -1
    member this.Position with get() = position and set value = position <- value