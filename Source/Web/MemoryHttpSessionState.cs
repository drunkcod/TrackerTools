using System;
using System.Collections.Generic;
using System.Web;

namespace TrackerTools.Web
{
    class MemoryHttpSessionState : HttpSessionStateBase
    {
        readonly Dictionary<string, object> items = new Dictionary<string, object>();

        public override object this[string name]{
            get {
                object item;
                if (items.TryGetValue(name, out item))
                    return item;
                return null;
            }
            set {
                items[name] = value;
            }
        }
    }
}
