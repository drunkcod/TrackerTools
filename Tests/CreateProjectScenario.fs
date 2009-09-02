namespace TrackerTools.Server
open TrackerTools
open NUnit.Framework
open System
open System.Net

module CreateProjectScenario =
    let [<Literal>] TrackerToken = ""
    let [<Literal>] TestService = "http://localhost:8110/"
    
    let mutable response : HttpWebResponse = null
    
    let [<SetUp>] Given() =
        let service = Tracker(TrackerToken, TestService)
        response <- service.CreateProject(TrackerProject(Name = "Test Project"))

    let [<Test>] StatusCode_should_be_201() =
            Assert.That((int)response.StatusCode, Is.EqualTo(201))

    let [<Test>] Location_should_be_a_valid_Uri() =
            let location = response.Headers.["Location"]
            Assert.That(Uri.IsWellFormedUriString(location, UriKind.Absolute),  Is.True, location)