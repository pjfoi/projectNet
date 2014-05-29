using System.Data.SqlClient;

namespace SamenSterkerData
{
    /// <summary>
    /// Interaction with the SamenSterker Database
    /// </summary>
    public static class SamenSterkerDB
    {
        /// <summary>
        /// Get a connection to the database.
        /// </summary>
        /// <returns>A connection to the database</returns>
        public static SqlConnection GetConnection()
        {
            string connectionString = 
                System.Configuration.ConfigurationManager
                .ConnectionStrings["LocalSqlServer"].ConnectionString;

            //switch (System.Environment.MachineName)
            //{
            //    case "PC_NEYENS":
            //        connectionString = @"Data Source=(localDB)\v11.0;AttachDbFilename=C:\Users\Peter\AB1_SQLserver_copy.mdf;Integrated Security=True";
            //        break;
            //    case "PJ":
            //        connectionString = "Data Source=(localDB)\\v11.0;AttachDbFilename=D:\\software\\Dropbox\\Geïntegreerd project\\NET project\\DB\\AB1_SQLserver.mdf;Integrated Security=True";
            //        break;
            //    case "M-HP":
            //        connectionString = "Data Source=(localDB)\\v11.0;AttachDbFilename=C:\\DB\\AB1_SQLserver.mdf;Integrated Security=True";
            //        break;
            //    case "latitude":
            //        connectionString = "Data Source=(localDB)\\v11.0;AttachDbFilename=C:\\Bestanden\\AB1_SQLserver.mdf;Integrated Security=True";
            //        break;
            //}

            return new SqlConnection(connectionString);
        }
    }
}
