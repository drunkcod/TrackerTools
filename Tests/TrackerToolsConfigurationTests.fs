namespace TrackerTools
open System
open System.Configuration
open NUnit.Framework

module TrackerToolsConfigurationTests =
    let [<Test>] should_throw_sensible_exception_when_GetSection_fails() =
        let section = ConfigurationManager.GetSection(TrackerToolsConfiguration.SectionName)
        Assert.IsNull(section, "There shouldn't be a section present for the test suite.")
        Assert.Throws(typeof<ArgumentException>,  fun () -> TrackerToolsConfiguration.FromAppConfig() |> ignore) |> ignore
