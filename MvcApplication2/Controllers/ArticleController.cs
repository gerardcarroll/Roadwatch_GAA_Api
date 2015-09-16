using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.ApplicationInsights;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class ArticleController : ApiController
    {
#if DEBUG
        private const string ConnectionString =
            "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
            private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#endif

        private static MySqlConnection _connection;

        public IEnumerable<Article1> GetArticles()
        {
            var tc = new TelemetryClient();
            tc.TrackEvent("ArticlesPageHit");

            var articles = new List<Article1>();
            try
            {
                //newArticles = GetXmlArticles();
                //foreach (var newArticle in newArticles.Article)
                //{
                //    Article1 article = new Article1
                //        {
                //            content_image_id = newArticle.content_image_id.ToString(),
                //            content_text_id = newArticle.content_text_id.ToString(),
                //            filename = newArticle.filename,
                //            ID = newArticle.id,
                //            thumbnail = newArticle.thumbnail,
                //            title = WebUtility.HtmlDecode(newArticle.title),
                //            upload_date = DateTime.Parse(newArticle.upload_date),
                //            url = newArticle.url,
                //            code = "General"
                //        };
                //    articles.Add(article);
                //    if (article.title.StartsWith("LIVE:") || article.title.Contains("Live!"))
                //    {
                //        InsertUpdateToDb(article.ID.ToString());
                //    }
                //}

                //To change comment from here
                var articlesNotInDb = new List<Article1>();
                var articlesInDb = new List<Article1>();
                var json = "";
                var newCode = "";

                using (var client = new WebClient())
                {
                    json = client.DownloadString("http://www.gaa.ie/iphone/get_news_json.php");
                }
                json = json.Replace("@attributes", "attributes");
                json = json.Remove(0, 11);
                json = json.TrimEnd('}');
                var articless = JsonConvert.DeserializeObject<List<Article>>(json);


                foreach (var a in articless)
                {
                    if (a.newsid != "12370")
                    {
                        var isFootball = false;
                        foreach (var sec in a.sections.section_id)
                        {
                            if (sec == "13")
                            {
                                newCode = "Camogie";
                                break;
                            }
                            if (sec == "1")
                            {
                                newCode = "General";
                                continue;
                            }
                            if (sec == "2")
                            {
                                newCode = "Football";
                                isFootball = true;
                                continue;
                            }
                            if (sec == "3" && isFootball)
                            {
                                newCode = "General";
                                continue;
                            }
                            if (sec == "3")
                            {
                                newCode = "Hurling";
                            }
                        }
                        a.filename = a.filename.Replace("http:", "");
                        if (a.title.StartsWith("LIVE:") || a.title.Contains("Live!") || a.title.Contains("LIVE!") ||
                            a.title.EndsWith("as it happens"))
                        {
                            InsertUpdateToDb(a.newsid);
                        }
                        var article = new Article1
                        {
                            content_image_id = a.content_image_id,
                            content_text_id = a.content_text_id,
                            filename = a.filename,
                            ID = Convert.ToInt32(a.newsid),
                            thumbnail = a.thumbnail,
                            title = WebUtility.HtmlDecode(a.title),
                            upload_date = DateTime.Parse(a.upload_date),
                            url = a.url,
                            code = newCode
                        };
                        articles.Add(article);
                    }
                } //To Here

                //Get Articles from DB
                articlesInDb = GetArticlesFromDb();
                //Change date based on db
                foreach (var article1 in articles)
                {
                    var q = (from ar in articlesInDb
                        where ar.ID == article1.ID
                        select ar).FirstOrDefault();
                    if (q != null)
                    {
                        article1.upload_date = q.upload_date;
                    }
                    else
                    {
                        article1.upload_date = DateTime.Now;
                        articlesNotInDb.Add(article1);
                    }
                }

                foreach (var article1 in articlesNotInDb)
                {
                    articles.Remove(article1);
                }

                //if new save to db
                SaveToDb(articlesNotInDb);

                articles.AddRange(articlesNotInDb);
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return articles.OrderByDescending(a => a.ID).Take(25);
        }

        private void SaveToDb(List<Article1> articlesNotInDb)
        {
            try
            {
                const string query = "INSERT INTO GAA_Article (Id, Upload_Date) VALUES (@id, @date);";

                foreach (var article1 in articlesNotInDb)
                {
                    using (_connection)
                    {
                        _connection.Open();
                        using (var cmd = new MySqlCommand(query, _connection))
                        {
                            // Start using the passed values in our parameters:
                            cmd.Parameters.AddWithValue("@id", article1.ID);
                            cmd.Parameters.AddWithValue("@date", article1.upload_date);

                            // Execute the query
                            cmd.ExecuteNonQuery();
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
        }

        private List<Article1> GetArticlesFromDb()
        {
            var articles = new List<Article1>();
            try
            {
                _connection = new MySqlConnection(ConnectionString);

                var query = "SELECT * from GAA_Article order by Id desc Limit 50;";

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                var article = new Article1();
                                article.ID = Convert.ToInt32(r.GetString("Id"));
                                article.upload_date = DateTime.Parse(r.GetString("Upload_Date"));
                                articles.Add(article);
                            }
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

            return articles;
        }

        public IEnumerable<Article1> GetArticlesWithTime(string lastLogin)
        {
            var articles = new List<Article1>();
            var json = "";
            var newCode = "Uploaded: ";

            try
            {
                using (var client = new WebClient())
                {
                    json = client.DownloadString("http://www.gaa.ie/iphone/get_news_json.php");
                }
                json = json.Replace("@attributes", "attributes");
                json = json.Remove(0, 11);
                json = json.TrimEnd('}');
                var articless = JsonConvert.DeserializeObject<List<Article>>(json);

                foreach (var a in articless)
                {
                    var isFootball = false;
                    //bool isHurling = false;
                    foreach (var sec in a.sections.section_id)
                    {
                        if (sec == "1")
                        {
                            newCode = "General";
                            continue;
                        }
                        if (sec == "2")
                        {
                            newCode = "Football";
                            isFootball = true;
                            continue;
                        }
                        if (sec == "3" && isFootball)
                        {
                            newCode = "General";
                            //isHurling = true;
                            continue;
                        }
                        if (sec == "3")
                        {
                            newCode = "Hurling";
                            //isHurling = true;
                        }
                        //if (sec == "11")
                        //{
                        //    newCode = "Top Daily";
                        //    break;
                        //}

                        // newCode = "General";
                    }
                    a.filename = a.filename.Replace("http:", "");
                    if (a.title.StartsWith("LIVE:") || a.title.Contains("Live!"))
                    {
                        InsertUpdateToDb(a.newsid);
                    }
                    var article = new Article1
                    {
                        content_image_id = a.content_image_id,
                        content_text_id = a.content_text_id,
                        filename = a.filename,
                        ID = Convert.ToInt32(a.newsid),
                        thumbnail = a.thumbnail,
                        title = WebUtility.HtmlDecode(a.title),
                        upload_date = DateTime.Parse(a.upload_date),
                        url = a.url,
                        code = newCode,
                        New = false
                    };
                    articles.Add(article);
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            foreach (var article1 in articles)
            {
                if (article1.upload_date > DateTime.Parse(lastLogin))
                {
                    article1.New = true;
                }
            }

            return articles;
        }

        private void InsertUpdateToDb(string p)
        {
            try
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();
                var query = "UPDATE db1305421_wpdev.UpdateTbl SET UpdateID='" + p + "', Date= '" + DateTime.Now +
                            "' WHERE Id = 1 ;";

                using (var cmd = new MySqlCommand(query, _connection))
                {
                    cmd.ExecuteNonQuery();
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
                _connection.Close();
            }
        }

        private Articles GetXmlArticles()
        {
            var ser = new XmlSerializer(typeof (Articles));
            Articles articles;
            using (var reader = XmlReader.Create("http://www.gaa.ie/iphone/get_news_xml.php?section=1"))
            {
                articles = (Articles) ser.Deserialize(reader);
            }
            return articles;
        }
    }
}