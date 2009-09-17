namespace TrackerTools
open System

[<AttributeUsage(AttributeTargets.Class)>]
type CommandNameAttribute(name:string) = 
    inherit Attribute()
    member this.Name =  name