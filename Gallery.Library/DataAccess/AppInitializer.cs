using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gallery.Library.DataAccess
{
    //CreateDatabaseIfNotExists is implicitly used when there is no other initalizer defined. 
    public class AppInitializer : CreateDatabaseIfNotExists<AppContext>
    {
    }
}