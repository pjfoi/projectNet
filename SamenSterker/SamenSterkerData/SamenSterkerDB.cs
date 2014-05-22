using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public static class SamenSterkerDB
    {
        public static SqlConnection GetConnection()
        {
            string connectionString =
            "Data Source=(localDB)\\v11.0;AttachDbFilename=C:\\DB\\AB1_SQLserver.mdf;Integrated Security=True";

            switch (System.Environment.MachineName)
            {
                case "PC_NEYENS":
                    connectionString = @"Data Source=(localDB)\v11.0;AttachDbFilename=C:\Users\Peter\AB1_SQLserver_copy.mdf;Integrated Security=True";
                    break;
                case "PJ":
                    connectionString = "Data Source=(localDB)\\v11.0;AttachDbFilename=D:\\software\\Dropbox\\Geïntegreerd project\\NET project\\DB\\AB1_SQLserver.mdf;Integrated Security=True";
                    break;
                case "M-HP":
                    connectionString = "Data Source=(localDB)\\v11.0;AttachDbFilename=C:\\DB\\AB1_SQLserver.mdf;Integrated Security=True";
                    break;
                case "latitude":
                    connectionString = "Data Source=(localDB)\\v11.0;AttachDbFilename=C:\\Bestanden\\AB1_SQLserver.mdf;Integrated Security=True";
                    break;
            }

            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
