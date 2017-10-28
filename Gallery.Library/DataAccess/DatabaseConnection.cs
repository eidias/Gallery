using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Library.DataAccess
{
    //Connection strings change very rarely and are good candidates to be kept in code.
    public class DatabaseConnection
    {
        public static readonly SqlConnectionStringBuilder Local = new SqlConnectionStringBuilder
        {
            ApplicationName = AppDomain.CurrentDomain.FriendlyName,
            DataSource = "(local)",
            ConnectTimeout = 30,
            //For EF it should not be necessary to use MARS.
            MultipleActiveResultSets = false,
            //The option to use the Security Support Provider Interface (SSPI) or specify a trusted connection is missing as they are not required for SQL Server.
            IntegratedSecurity = true,
            InitialCatalog = "MyDatabase",
        };
    }
}
