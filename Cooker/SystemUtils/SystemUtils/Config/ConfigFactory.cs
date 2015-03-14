using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace Cooker
{
    namespace SystemUtils
    {
        public class ConfigFactory
        {
            public static Config CreateConfig(string connectString, string company, string software)
            {
                return new SystemConfig(connectString, company, software);
            }
            public static Config CreateConfig(SqlConnection connection, string company, string software)
            {
                return new SystemConfig(connection.ConnectionString, company, software);
            }
        }
    }
}
