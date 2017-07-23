using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Gallery.Library.DataAccess
{
    //CreateDatabaseIfNotExists is implicitly used when there is no other initalizer defined. 
    public class AppInitializer : CreateDatabaseIfNotExists<AppContext>
    {
        Stream stream;

        public AppInitializer()
        {
        }

        //Allows database seeding from a stream.
        public AppInitializer(Stream stream)
        {
            this.stream = stream;
        }

        protected override void Seed(AppContext context)
        {
            if(stream == null)
            {
                return;
            }

            //Implement method to deserialize the stream and seed the database.
        }

    }
}