namespace TrackerTools.Server
open System.Web.Routing
open System.Web.Mvc
open TrackerTools.Web
open NUnit.Framework

module WhenCreatingProject = 
    let [<Test>] should_return_201() =
        MvcApplication.RegisterRoutes(RouteTable.Routes)
        let factory = BasicControllerFactory()
        ControllerBuilder.Current.SetControllerFactory(factory);

        let response = BasicHttpResponse.From(BasicHttpRequest.Post("http://tracker/Projects"), RouteTable.Routes)
        Assert.That(response.StatusCode, Is.EqualTo(201))