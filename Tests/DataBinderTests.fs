namespace TrackerTools
open System
open System.Xml.Serialization
open NUnit.Framework

type Item() =
    let mutable someProperty = String.Empty
    [<XmlElement("some_property")>]
    member this.SomeProperty with get() = someProperty and set(value) = someProperty <- value
    [<DefaultValue; XmlElement("some_field")>]
    val mutable SomeField : string


module DataBinderTests =

    [<Test>]
    let should_support_binding_to_public_properties() =
        Assert.That(DataBinder.Bind("$(SomeProperty)", Item(SomeProperty = "42"), string), Is.EqualTo("42"))

    [<Test>]
    let should_support_binding_Properties_via_XmlElement() =
        Assert.That(DataBinder.Bind("$(some_property)", Item(SomeProperty = "HelloWorld"), string), Is.EqualTo("HelloWorld"))

    [<Test>]
    let shuld_support_binding_via_public_fields() =
        Assert.That(DataBinder.Bind("$(SomeField)", Item(SomeField = "7"), string), Is.EqualTo("7"))

    [<Test>]
    let shuld_support_binding_fields_via_XmlElement() =
        Assert.That(DataBinder.Bind("$(some_field)", Item(SomeField = "Foo"), string), Is.EqualTo("Foo"))
