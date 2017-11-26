using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Hangfire.Data
{
    /// <summary>
    /// Contains helper methods to work with SQL Server for scenarios not covered by Entity Framework.
    /// </summary>
    public class SqlServerHelper
    {
        public const string DatabaseName = "Demo.Hangfire";

        /// <summary>
        /// Returns the root path of the filetable implementation for the given connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>Physical path as string in UNC notion.</returns>
        public static string GetFileTableRootPath(string connectionString) => ExecuteScalar(connectionString, "SELECT FileTableRootPath()") as string;

        static object ExecuteScalar(string connectionString, string cmdText)
        {
            //By default uses a connection from the pool and returns it at the end of the using statement.
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
                return sqlCommand.ExecuteScalar();
            }
        }

        public static string DefaultConnectionString => defaultConnection.ConnectionString;

        static readonly SqlConnectionStringBuilder defaultConnection = new SqlConnectionStringBuilder
        {
            DataSource = "(local)",
            //Will show the executable name or for web applications the HttpRuntime.AppDomainId in the IIS metabase format /LM/W3SVC/2/ROOT-1-131523697902554787	
            ApplicationName = AppDomain.CurrentDomain.FriendlyName,
            InitialCatalog = DatabaseName,
            IntegratedSecurity = true
        };
    }
}
