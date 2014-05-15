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
            "Data Source=(localDB)\\v11.0;AttachDbFilename=C:\\Bestanden\\AB1_SQLserver.mdf;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
