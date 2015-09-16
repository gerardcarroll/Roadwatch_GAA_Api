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
    public class GalleryController : ApiController
    {
#if DEBUG
        private const string ConnectionString =
            "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
                    private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif
        private MySqlConnection _connection;
        private readonly List<Gallery> _galleriesNotInDb = new List<Gallery>();

        // GET api/gallery
        public IEnumerable<Gallery> GetGalleries()
        {
            var galleries = new List<Gallery>();

            try
            {
                //Scrape galleries from site

                //galleries = ScrapeGalleries();

                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();

                foreach (var g in galleries)
                {
                    var sql = string.Format("SELECT * from Gallery " +
                                            "where Title = '{0}' order by ID desc", g.Title);

                    using (var cmd = new MySqlCommand(sql, _connection))
                    {
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.HasRows)
                            {
                                _galleriesNotInDb.Add(g);
                            }
                        }
                    }
                }

                if (_galleriesNotInDb.Count > 0)
                {
                    foreach (var gallery in _galleriesNotInDb)
                    {
                        InsertGalleryToDb(gallery);
                    }
                }

                //Get all galleries new and old
                galleries = GetAllGalleriesFromDb();
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            finally
            {
                _connection.Close();
            }

            return galleries;
        }

        private static List<Gallery> ScrapeGalleries()
        {
            var galleries = new List<Gallery>();
            var web = new HtmlWeb();

            var doc = web.Load("http://www.gaa.ie/gaa-news-and-videos/image-gallery/");

            var nodes = doc.DocumentNode.SelectNodes("//div[@id='image_gallery']").Descendants();
            nodes = doc.DocumentNode.SelectNodes("//div[@class='gallery clearfix']");
            foreach (var node in nodes)
            {
                var gallery2 = new Gallery();
                var decodedString = WebUtility.HtmlDecode(node.ChildNodes[1].InnerText);
                gallery2.Title = Regex.Replace(decodedString,
                    @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<br>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>", "");

                gallery2.Img = "http://gaa.ie";
                gallery2.Link = "http://gaa.ie";
                galleries.Add(gallery2);
                if (node.Name != "ul") continue;
                var liNodes = node.ChildNodes;
                foreach (var nodeLi in liNodes)
                {
                    if (nodeLi.Name != "li") continue;
                    var gallery = new Gallery();

                    //imgNode = nodeLi.ChildNodes[1].ChildNodes[1].Attributes;
                    //foreach (var htmlAttribute in imgNode.Where(htmlAttribute => htmlAttribute.Name == "src"))
                    //{
                    //    gallery.Img = "http://gaa.ie" + htmlAttribute.Value;
                    //}

                    decodedString = WebUtility.HtmlDecode(nodeLi.InnerText);
                    gallery.Title = Regex.Replace(decodedString,
                        @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<br>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>",
                        "");

                    IEnumerable<HtmlAttribute> attr = nodeLi.ChildNodes[1].Attributes;
                    foreach (var attribute in attr.Where(attribute => attribute.Name == "href"))
                    {
                        gallery.Link = "http://gaa.ie" + new Uri(attribute.Value, UriKind.RelativeOrAbsolute);
                    }
                    galleries.Add(gallery);
                }
            }
            foreach (var gallery in galleries)
            {
                gallery.Tile = GetTileImage(gallery);
                //galleries.Add(gallery);
            }
            return galleries;
        }

        private static String GetTileImage(Gallery gallery)
        {
            var web = new HtmlWeb();
            var doc = web.Load(gallery.Link);

            var nodes = doc.DocumentNode.SelectNodes("//div[@id='gallery-slider']").Descendants();

            foreach (var node in nodes)
            {
                if (node.Name != "a") continue;
                //var image = new AppImage { Title = title };
                //var decodedString = WebUtility.HtmlDecode(node.InnerHtml);
                //image.Abstract = Regex.Replace(decodedString,
                //    @"\t|\n|\r", "");
                //image.Abstract = Regex.Replace(image.Abstract,
                //   @"<span>|</span>", "\t\n");
                var attr = node.Attributes;
                var first = (from a in attr where a.Name == "href" select a.Value).FirstOrDefault();
                gallery.Tile = "http://gaa.ie" + first;
                return gallery.Tile;
            }

            return null;
        }

        private List<Gallery> GetAllGalleriesFromDb()
        {
            var galleries = new List<Gallery>();

            try
            {
                const string sql = "Select * from Gallery order by ID desc";
                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var g = new Gallery
                            {
                                Title = r.GetString("Title"),
                                Link = r.GetString("Link"),
                                Img = r.GetString("Image"),
                                Tile = r.GetString("Tile")
                            };
                            galleries.Add(g);
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

            return galleries;
        }

        private void InsertGalleryToDb(Gallery g)
        {
            try
            {
                const string query =
                    "INSERT INTO Gallery (Title, Link, Image, Tile) VALUES (@title, @link, @image, @tile);";

                using (var cmd = new MySqlCommand(query, _connection))
                {
                    // Start using the passed values in our parameters:
                    cmd.Parameters.AddWithValue("@title", g.Title);
                    cmd.Parameters.AddWithValue("@link", g.Link);
                    cmd.Parameters.AddWithValue("@image", g.Img);
                    cmd.Parameters.AddWithValue("@tile", g.Tile);

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
    }
}