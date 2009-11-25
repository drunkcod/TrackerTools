namespace TrackerTools
open NUnit.Framework

type SampleType() =
    member this.Arg0<'a>([<FromCommandLine(Position = 0)>] x:'a) = ()
    member this.Method1([<FromCommandLine(Position = 1)>] x:int) = ()

module CommandLineParameterBinderTests =
    let fromArg1 = typeof<SampleType>.GetMethod("Method1").GetParameters().[0]
    let arg0<'a> = typeof<SampleType>.GetMethod("Arg0").MakeGenericMethod([|typeof<'a>|]).GetParameters().[0]

    let [<Test>] should_support_positional_binding() =
        let binder = CommandLineParameterBinder([|"1"; "2"; "3"|])
        let bound = binder.Bind(fromArg1)
        Assert.That(bound, Is.EqualTo(2))

    let [<Test>] should_support_hard_positional_binding() =
        let binder = CommandLineParameterBinder([|"1"; "2"; "3"|])
        Assert.That(binder.Bind(1, typeof<int>), Is.EqualTo(2))

    let [<Test>] should_support_typed_binding() =
        let binder = CommandLineParameterBinder([|"1"; "2"; "3"|])
        Assert.That(binder.Bind<int>(1), Is.EqualTo(2))

    let [<Test>] should_support_string_parameters() =
        let binder = CommandLineParameterBinder([|"Hello World"|])
        let bound = binder.Bind(arg0<string>)
        Assert.That(bound, Is.EqualTo("Hello World"))

    let [<Test>] should_supoprt_int_parameters() =
        let binder = CommandLineParameterBinder([|"42"|])
        let bound = binder.Bind(arg0<int>)
        Assert.That(bound, Is.EqualTo(42))

    let [<Test>] should_ignore_options_during_positional_binding() =
        let binder = CommandLineParameterBinder([|"--option=Foo"; "42"|])
        let bound = binder.Bind(arg0<int>)
        Assert.That(bound, Is.EqualTo(42))

    let [<Test>] should_support_named_options() =
        let binder = CommandLineParameterBinder([|"--option=Value"|])
        Assert.That(binder.GetOption("option"), Is.EqualTo("Value"))
