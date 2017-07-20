using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gallery.Web.HttpModules
{
    public class ObjectContextModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += OnEndRequest;
        }

        private void OnEndRequest(object sender, EventArgs eventArgs)
        {
            if (HttpContext.Current.Items["ObjectContext"] != null)
            {
                var objectContext = HttpContext.Current.Items["ObjectContext"] as IDisposable;
                objectContext?.Dispose();
            }
        }

        public void Dispose()
        {

        }
    }
}