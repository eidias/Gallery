using Gallery.Library.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Gallery.Library.DataAccess
{
    //Naming is generalized and in line with e.g. App_Start
    public class AppContext : DbContext
    {
        //The context is not thread safe -> ensure instances of the same entity class are not tracked by multiple contexts at the same time.
        public AppContext()
        {

        }

        //When working with Web applications, use a context instance per request.
        public AppContext(DbConnectionStringBuilder dbConnectionStringBuilder)
            : base(dbConnectionStringBuilder.ConnectionString)
        {
        }

        public virtual DbSet<Entity> Entities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Enable "Redirect all Output Window text to Immediate Window.
            Database.Log = x => Debug.Write(x);

            modelBuilder.Properties<string>().Configure(s => s.HasMaxLength(8000).HasColumnType("varchar"));
        }
    }
}