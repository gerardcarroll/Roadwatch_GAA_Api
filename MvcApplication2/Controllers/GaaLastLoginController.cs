using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class GaaLastLoginController : ApiController
    {
#if DEBUG
        private const string ConnectionString = "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
            private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#endif

        public string GetLastLogin(string id)
        {
            string s = "";

            try
            {
                string query = string.Format("SELECT * from WP_GAA_Users where Unique_ID like '{0}';", id);

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                var loginOrdinal = r.GetOrdinal("Last_Used");
                                if (r.IsDBNull(loginOrdinal)) s = DateTime.MinValue.ToString();
                                else s = r.GetString("Last_Used");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                MethodBase methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return s;
        }
    }
}
