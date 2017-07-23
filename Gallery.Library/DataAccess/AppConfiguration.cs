using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

//Place your DbConfiguration class in the same assembly as your DbContext class. (See the Moving DbConfiguration section if you want to change this.)
namespace Gallery.Library.DataAccess
{
    //Create only one DbConfiguration class for your application. This class specifies app-domain wide settings.
    public class AppConfiguration : DbConfiguration
    {
        //Give your DbConfiguration class a public parameterless constructor.
        public AppConfiguration()
        {
            //Corresponds to the defaultConnectionFactory section in the web.config file
            SqlConnectionFactory sqlConnectionFactory = new SqlConnectionFactory();
            SetDefaultConnectionFactory(sqlConnectionFactory);

            //Corresponds to the providers section in the web.config file
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);

            AppInitializer appInitializer = new AppInitializer();
            SetDatabaseInitializer(appInitializer);
        }
    }
}