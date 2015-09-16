using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using HtmlAgilityPack;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class ParkingController : ApiController
    {
#if DEBUG
        private const string ConnectionString =
            "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
                    private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif
        private MySqlConnection _connection;
        private string openingHours = "";
        private string tariffs = "";

        public List<ParkingCity> GetCities()
        {
            var cities = new List<ParkingCity>();

            try
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();

                var sql = string.Format("SELECT * from Roadwatch_Parking_City ");

                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var city = new ParkingCity
                            {
                                Id = r.GetInt32("Id"),
                                Name = r.GetString("Name")
                            };
                            cities.Add(city);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return cities;
        }

        public List<Carpark> GetCarparks(string name, int id)
        {
            var carparks = new List<Carpark>();
            var recent = true;
            var getThem = false;
            try
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();

                var sql = string.Format("SELECT * from Roadwatch_Parking where City = '{0}' ", name);

                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                //if (DateTime.Now.Subtract(r.GetDateTime("Updated")).TotalDays > 30)
                                //{
                                //    recent = false;
                                //    break;
                                //}
                                var cp = new Carpark
                                {
                                    Access = r.GetString("Access"),
                                    City = r.GetString("City"),
                                    CityId = r.GetInt32("City_Id"),
                                    District = r.GetString("District"),
                                    Hours = r.GetString("Hours"),
                                    Id = r.GetInt32("Id"),
                                    Location = r.GetString("Location"),
                                    Name = r.GetString("Name"),
                                    Tarrifs = r.GetString("Tarriffs"),
                                    Url = r.GetString("Url")
                                };
                                carparks.Add(cp);
                            }
                        }
                        else
                        {
                            getThem = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            if (!recent || getThem)
            {
                var web = new HtmlWeb();

                var doc = web.Load("http://www.theaa.ie/AA/AA-Roadwatch/Car-parking/" + name + "/");

                var nodes = doc.DocumentNode.SelectNodes("//div[@class='parkingList']");

                foreach (var node in nodes)
                {
                    var parks = node.ChildNodes[3].ChildNodes[3].ChildNodes; //.SelectNodes("//tr");
                    foreach (var n in parks.Where(p => p.Name == "tr"))
                    {
                        var cp = new Carpark();
                        cp.District = CleanString(node.ChildNodes[1].InnerText);
                        cp.Name = CleanString(n.ChildNodes[1].InnerText);
                        cp.Location = CleanString(n.ChildNodes[3].InnerText);
                        cp.Url = "http://www.theaa.ie" + n.ChildNodes[1].ChildNodes[1].Attributes[0].Value;
                        cp.Access = CleanString(GetDetails(cp.Url));
                        cp.Tarrifs = CleanString(tariffs);
                        cp.Hours = CleanString(openingHours);
                        cp.City = name;
                        cp.CityId = id;

                        carparks.Add(cp);
                    }
                }

                if (carparks.Count > 0)
                {
                    foreach (var cp in carparks)
                    {
                        //InsertCarparkToDb(cp);
                    }
                }

                GetCarparks(name, id);
            }

            return carparks;
        }

        private string CleanString(string s)
        {
            string clean = "", text = "", decodedString = "";

            text = WebUtility.HtmlDecode(s);
            decodedString = Regex.Replace(text,
                @"\t|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>",
                "");
            decodedString = decodedString.Replace("<br> ", "\n");
            clean = decodedString.Trim();
            return clean;
        }

        private void InsertCarparkToDb(Carpark cp)
        {
            _connection = new MySqlConnection(ConnectionString);
            _connection.Open();

            try
            {
                const string query =
                    "INSERT INTO Roadwatch_Parking (City, District, Name, Location, Url, Access, Hours, Tarriffs, City_Id, Updated)" +
                    " VALUES (@city, @district, @name, @location, @url, @access, @hours, @tarriffs, @cityid, @updated);";

                using (var cmd = new MySqlCommand(query, _connection))
                {
                    // Start using the passed values in our parameters:
                    cmd.Parameters.AddWithValue("@city", cp.City);
                    cmd.Parameters.AddWithValue("@district", cp.District);
                    cmd.Parameters.AddWithValue("@name", cp.Name);
                    cmd.Parameters.AddWithValue("@location", cp.Location);
                    cmd.Parameters.AddWithValue("@url", cp.Url);
                    cmd.Parameters.AddWithValue("@access", cp.Access);
                    cmd.Parameters.AddWithValue("@hours", cp.Hours);
                    cmd.Parameters.AddWithValue("@tarriffs", cp.Tarrifs);
                    cmd.Parameters.AddWithValue("@cityid", cp.CityId);
                    cmd.Parameters.AddWithValue("@updated", DateTime.Now);

                    // Execute the query
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }

        private string GetDetails(string p)
        {
            var access = "";
            openingHours = "";
            tariffs = "";
            var web = new HtmlWeb();
            var doc = web.Load(p);
            var nodes = doc.DocumentNode.SelectNodes("//table[@class='parkingTable']");
            foreach (var node in nodes.Where(n => n.Name == "table"))
            {
                access = node.ChildNodes[3].ChildNodes[3].ChildNodes[3].InnerHtml;
                openingHours = node.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerHtml;
                tariffs = node.ChildNodes[3].ChildNodes[7].ChildNodes[3].InnerHtml;
            }

            return access;
        }
    }
}