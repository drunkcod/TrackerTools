namespace TrackerTools.Web
open System.Web
open System.Web.Routing
open NUnit.Framework

module HttpHandlerRouteTests =
    let [<Test>] Wont_recreate_handler_if_reusable() =
        let route = HttpHandlerRoute(fun () -> {new IHttpHandler with
            member this.IsReusable = true
            member this.ProcessRequest context = ()}) :> IRouteHandler
        Assert.That(route.GetHttpHandler(null), Is.SameAs(route.GetHttpHandler(null)))
