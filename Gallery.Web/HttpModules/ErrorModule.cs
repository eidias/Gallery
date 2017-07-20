using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gallery.Web.HttpModules
{
    public class ErrorModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(OnError);
        }

        void OnError(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication)sender;
            HttpContext httpContext = httpApplication.Context;
            try
            {
                HttpException httpException = httpContext.Error as HttpException;
                if (httpException != null)
                {
                    // Handle error here, preferably write the error to the database.

                    // Maybe even clear the error depending on what error it is (e.g. portenitally dangerous request parameter).
                    httpContext.ClearError();

                    // We could forward the errors we could not handle directly to an error page.
                    httpContext.Server.Transfer("/ErrorMessage.aspx");
                }
            }
            catch
            {
                // We need be able to write to the server error log e.g. in case the database is full.
            }
        }

        public void Dispose()
        {
        }
    }
}