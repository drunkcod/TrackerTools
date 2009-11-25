namespace TrackerTools.Server
open TrackerTools
open NUnit.Framework
open System
open System.Text
open System.IO
open System.Net
open System.Xml.Serialization

[<Category("Scenario")>]
module CreateProjectScenario =
    let [<Literal>] TrackerToken = ""
    let [<Literal>] TestService = "http://localhost:8110/"

    let testProject = TrackerProject(Name = "Test Project")
    let mutable Location = String.Empty
    let mutable StatusCode = -1
    let mutable Response : TextReader = null
    let response = {new IResponseHandler with
        member this.HandleResponse x =
            Location <- x.Headers.["Location"]
            StatusCode <- (int)x.StatusCode
            use reader = new StreamReader(x.GetResponseStream())
            Response <- new StringReader(reader.ReadToEnd())}

    let [<TestFixtureSetUp>] When_I_create_a_project() =
        let service = Tracker(TrackerToken, TestService)
        service.CreateProject testProject response

    let [<Test>] StatusCode_should_be_201() =
            Assert.That(StatusCode, Is.EqualTo(201))

    let [<Test>] Location_should_be_a_valid_Uri() =
        Assert.That(Uri.IsWellFormedUriString(Location, UriKind.Absolute),  Is.True, Location)

    let [<Test>] Response_body_should_be_a_matching_project() =
        let serializer = XmlSerializer(typeof<TrackerProject>)
        let reply = serializer.Deserialize(Response) :?> TrackerProject
        Assert.That(reply.Name, Is.EqualTo(testProject.Name))