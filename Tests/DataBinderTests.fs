namespace TrackerTools
open System
open NUnit.Framework

module DataBinder =
    let Bind(format:string, item) = 
        let result = ref format
        item.GetType().GetProperties()
        |> Seq.iter (fun prop -> result := (!result).Replace(String.Format("$({0})", prop.Name), prop.GetValue(item, null).ToString()))
        !result

module DataBinderTests =
    type Item() =
        let mutable someProperty = String.Empty
        member this.SomeProperty with get() = someProperty and set(value) = someProperty <- value
 
    [<Test>]
    let should_support_binding_to_public_properties() =
        Assert.That(DataBinder.Bind("$(SomeProperty)", Item(SomeProperty = "42")), Is.EqualTo("42"))