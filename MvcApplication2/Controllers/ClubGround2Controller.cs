using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using HtmlAgilityPack;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class ClubGround2Controller : ApiController
    {
#if DEBUG
        private const string ConnectionString = "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
            private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#endif

        public List<ClubGround> GetClubGroundByCounty(string county)
        {
            List<ClubGround> clubGrounds = new List<ClubGround>();
            MySqlConnection _connection;
            _connection = new MySqlConnection(ConnectionString);

            try
            {
                _connection.Open();
                string sql = "Select * from Club_Grounds where County = '" + county + "' order by Club_Ground";

                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var v = new ClubGround
                                    {
                                        Id = Convert.ToInt32(r.GetString("Id")),
                                        Club_Ground = r.GetString("Club_Ground"),
                                        County = county,
                                        Latitude = r.GetString("Latitude"),
                                        Longitude = r.GetString("Longitude"),
                                        Club_Ground_2 = r.GetString("Club_Ground_2"),
                                        Colours = r.GetString("Colours"),
                                        Email = r.GetString("Email"),
                                        Facebook = r.GetString("Facebook"),
                                        Phone = r.GetString("Phone"),
                                        Website = r.GetString("Website"),
                                        Twitter = r.GetString("Twitter")
                                    };
                            clubGrounds.Add(v);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return clubGrounds;
        }

        [HttpGet]
        public String ScrapeClubGrounds(string county, string id)
        {
            List<ClubGround> grounds = new List<ClubGround>();
            MySqlConnection _connection;
            _connection = new MySqlConnection(ConnectionString);
            var web = new HtmlWeb();
            HtmlDocument doc;
            try
            {
                var link = "http://clubfinder.co/includes/clubs.php?sport=1&region=" + id;
                doc = web.Load(link);
                var nodes = doc.DocumentNode.SelectNodes("//button");
                _connection.Open();
                foreach (var node in nodes)
                {
                    var attr = node.Attributes;
                    foreach (var at in attr.AttributesWithName("onclick"))
                    {
                        string s = at.Value;
                        s = Regex.Match(s, @"\(([^;]*)\)").Groups[1].Value;
                        s = s.Replace("'", "");
                        var values = s.Split(',');
                        ClubGround cg = new ClubGround
                                        {
                                            Club_Ground = values[0],
                                            Club_Ground_2 = values[1],
                                            Latitude = values[2],
                                            Longitude = values[3],
                                            Colours = values[4],
                                            Website = values[5],
                                            Facebook = values[6],
                                            Twitter = values[7],
                                            Email = values[8],
                                            Phone = values[9],
                                            County = county
                                        };
                        grounds.Add(cg);

                        const string query =
                            "INSERT INTO Club_Grounds (County, Club_Ground, Club_Ground_2, Latitude, Longitude, Colours, Website, " +
                            "Facebook, Email, Phone, Twitter) VALUES (@county, @clubground, @clubground2, @latitude, @longitude, @colours, @website, " +
                            "@facebook, @email, @phone, @twitter);";

                        using (var cmd = new MySqlCommand(query, _connection))
                        {
                            // Start using the passed values in our parameters:
                            cmd.Parameters.AddWithValue("@county", cg.County);
                            cmd.Parameters.AddWithValue("@clubground", cg.Club_Ground);
                            cmd.Parameters.AddWithValue("@clubground2", cg.Club_Ground_2);
                            cmd.Parameters.AddWithValue("@latitude", cg.Latitude);
                            cmd.Parameters.AddWithValue("@longitude", cg.Longitude);
                            cmd.Parameters.AddWithValue("@colours", cg.Colours);
                            cmd.Parameters.AddWithValue("@website", cg.Website);
                            cmd.Parameters.AddWithValue("@facebook", cg.Facebook);
                            cmd.Parameters.AddWithValue("@email", cg.Email);
                            cmd.Parameters.AddWithValue("@phone", cg.Phone);
                            cmd.Parameters.AddWithValue("@twitter", cg.Twitter);

                            // Execute the query
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
                return "error";
            }
            finally
            {
                _connection.Close();
            }
            return "ok";
        }
    }
}
