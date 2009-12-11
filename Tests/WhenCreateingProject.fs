namespace TrackerTools.Server
open System.Web.Routing
open System.Web.Mvc
open TrackerTools
open TrackerTools.Web
open FNUnit

module WhenCreatingProject = 
    let [<Fact>] missing_body_should_return_400() =
        MvcApplication.RegisterRoutes(RouteTable.Routes)
        let factory = BasicControllerFactory()
        ControllerBuilder.Current.SetControllerFactory(factory);
        let response = BasicHttpResponse.From(BasicHttpRequest.Post("http://tracker/Projects"), RouteTable.Routes)
        response.StatusCode |> should be 400