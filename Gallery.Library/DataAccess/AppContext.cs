using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gallery.Library.DataAccess
{
    //Naming is generalized and in line with e.g. App_Start
    public class AppContext : DbContext
    {
        public AppContext()
        {

        }

        //The context is not thread safe.
        //You can still create a multithreaded application as long as an instance of the same entity class is not tracked by multiple contexts at the same time.
        //When working with Web applications, use a context instance per request.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}