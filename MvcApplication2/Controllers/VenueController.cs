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
    public class VenueController : ApiController
    {
        #if DEBUG
                private const string ConnectionString = "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #else
                    private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif
        public List<Venue> GetVenues()
        {
            var connection = new MySqlConnection(ConnectionString);
            var venues = new List<Venue>();
            
            try
            {
                connection.Open();
                
                const string sql = "Select * from Venue order by (CASE WHEN County='GAA HQ' THEN 0 ELSE 1 END), County";
                
                using (var cmd = new MySqlCommand(sql, connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var v = new Venue
                                    {
                                        ID = Convert.ToInt32(r.GetString("ID")),
                                        Club = r.GetString("Club"),
                                        County = r.GetString("County"),
                                        Latitude = r.GetString("Latitude"),
                                        Longitude = r.GetString("Longitude"),
                                        Name = r.GetString("Name"),
                                        Town = r.GetString("Town")
                                    };
                            venues.Add(v);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return venues;
        }
    }
}
