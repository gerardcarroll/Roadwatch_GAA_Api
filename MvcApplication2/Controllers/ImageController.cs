using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using HtmlAgilityPack;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class ImageController : ApiController
    {
        #if DEBUG
                private const string ConnectionString = "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #else
                    private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif
        readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);

        public IEnumerable<AppImage> GetImagesFromDb(string title, string link)
        {
            var images = new List<AppImage>();
            var imagesNotInDb = false;

            try
            {
                _connection.Open();

                var sql = string.Format("SELECT Location, Abstract from Image " +
                    "where Title = '{0}' order by ID", title);

                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                var img = new AppImage
                                          {
                                              URL = r.GetString("Location"),
                                              Abstract = r.GetString("Abstract")
                                          };
                                images.Add(img);
                            }
                        }
                        else
                        {
                            imagesNotInDb = true;
                        }
                    }
                }

                if (imagesNotInDb)
                {
                    //no images in db so insert them
                    GetImages(title, link);
                    //now get them and send back to phone
                    images = GetNewImages(title);
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

            return images;
        }

        private List<AppImage> GetNewImages(string title)
        {
            var images = new List<AppImage>();

            var sql = string.Format("SELECT * from Image " +
                    "where Title = '{0}' order by ID", title);

            try
            {
                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var img = new AppImage
                                      {
                                          Abstract = r.GetString("Abstract"),
                                          Title = r.GetString("Title"),
                                          URL = r.GetString("Location")
                                      };
                            images.Add(img);
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

            return images;
        }

        private void GetImages(string title, string link)
        {
            var images = new List<AppImage>();
            var web = new HtmlWeb();

            try
            {
                var doc = web.Load(link);

                var nodes = doc.DocumentNode.SelectNodes("//div[@id='gallery-slider']").Descendants();

                foreach (var node in nodes)
                {
                    if (node.Name != "a") continue;
                    var image = new AppImage { Title = title };
                    var decodedString = WebUtility.HtmlDecode(node.InnerHtml);
                    image.Abstract = Regex.Replace(decodedString,
                        @"\t|\n|\r", "");
                    image.Abstract = Regex.Replace(image.Abstract,
                        @"<span>|</span>", "\t\n");
                    if (image.Abstract != "\t\n\t\n")
                    {
                        if (image.Abstract.Contains(";"))
                        {
                            image.Abstract = image.Abstract.Substring(image.Abstract.IndexOf(';') + 1);
                        }
                        else if (image.Abstract.Contains(":"))
                        {
                            image.Abstract = image.Abstract.Substring(image.Abstract.IndexOf(':') + 1);
                        }
                        var attr = node.Attributes;
                        foreach (var attribute in attr.Where(attribute => attribute.Name == "href"))
                        {
                            image.URL = "http://gaa.ie" + attribute.Value;
                        }

                        images.Add(image);
                    }
                }

            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            try
            {
                foreach (var img in images)
                {
                    const string query = "INSERT INTO Image (Title, Location, Abstract) VALUES (@title, @location, @abstract);";

                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        // Start using the passed values in our parameters:
                        cmd.Parameters.AddWithValue("@title", img.Title);
                        cmd.Parameters.AddWithValue("@location", img.URL);
                        cmd.Parameters.AddWithValue("@abstract", img.Abstract);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

        }
    }
}
