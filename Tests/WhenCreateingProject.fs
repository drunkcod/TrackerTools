namespace TrackerTools.Server
open System
open System.IO
open System.Web.Routing
open System.Web.Mvc
open TrackerTools
open TrackerTools.Web
open FNUnit

module WhenCreatingProject =
    let [<Setup>] Setup() =
        let factory = BasicControllerFactory()
        ControllerBuilder.Current.SetControllerFactory(factory)

    let GetRouteTable() = 
        let routes = RouteCollection()
        MvcApplication.RegisterRoutes(routes)
        routes

    let [<Fact>] missing_body_should_return_400() =
        let response = BasicHttpResponse.From(BasicHttpRequest.Post("http://tracker/Projects"), GetRouteTable())
        response.StatusCode |> should be 400
    
    module valid_request =     
        let testProject = TrackerProject(Name = "Test Project")
        let body = XmlRequest(testProject)

        let [<Setup>] Setup() = 
            let factory = BasicControllerFactory()
            ControllerBuilder.Current.SetControllerFactory(factory)

        let [<Fact>] should_return_201() =
            let request = BasicHttpRequest.Post("http://tracker/Projects", body.ToStream())
            let response = BasicHttpResponse.From(request, GetRouteTable())
            response.StatusCode |> should be 201

        let [<Fact>] should_return_well_formed_Location_header() =
            let request = BasicHttpRequest.Post("http://tracker/Projects", body.ToStream())
            let response = BasicHttpResponse.From(request, GetRouteTable())
            Uri.IsWellFormedUriString(response.Headers.["Location"], UriKind.Absolute) |> should be true