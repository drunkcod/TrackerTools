using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Xlnt.Stuff;

namespace TrackerTools.Web
{
    public class BasicControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName){
            var wantedName = controllerName + "Controller";
            foreach (var item in Controllers)
                if (item.Name == wantedName)
                    return item.ConstructAs<IController>();
            throw new NotSupportedException();
        }

        IEnumerable<Type> Controllers {
            get {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    foreach (var type in assembly.GetTypes())
                        if (typeof(IController).IsAssignableFrom(type))
                            yield return type;
            }
        }
    }
}
