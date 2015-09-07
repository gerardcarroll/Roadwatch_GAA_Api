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
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class MatchTrackerController : ApiController
    {
        #if DEBUG
            private const string ConnectionString = "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #else
            private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif
        
        private static MySqlConnection _connection;
        public String GetMatchTracker()
        {
            String state = "off";

            string json = "";
            try
            {
                using (var client = new WebClient())
                {
                    json = client.DownloadString("http://www.gaa.ie/iphone/matchtracker_json.php");
                }
                var matchTrack = JsonConvert.DeserializeObject<MatchTracker>(json);

                state = matchTrack.State;
                if (state =="off")
                {
                    return "off";
                }
                UpdateTbl ut = GetId();
                double period = DateTime.Now.Subtract(ut.Date).TotalHours;
                if (state == "on" && period < 12)
                {
                    state = matchTrack.Url;
                }
                else
                {
                    state = "off";
                }
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
                state = "off";
            }
            
            return state;
        }

        public List<Update> GetUpdates(string url)
        {
            List<Update> updates = new List<Update>();
            UpdateTbl ut = new UpdateTbl();
            ut.Id = GetId().Id;
            url = "http://www.gaa.ie/modules/live_match.php?id=" + ut.Id;
            List<String> texts = new List<string>();
            string time = "";
            string score = "";
            var web = new HtmlWeb();
            HtmlDocument doc;
            try
            {
                var link = url;
                doc = web.Load(link);
               
                var nodes = doc.DocumentNode.SelectNodes("//div[@class='mt_updates']");

                //var count = nodes.Count();
                bool cont = true;
                foreach(var node in nodes)
                {
                    cont = true;
                    time = WebUtility.HtmlDecode(node.ChildNodes[0].InnerText);
                    time = Regex.Replace(time,
                        @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|<br>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>", "");
                    
                    var attr = node.ChildNodes;
                    foreach (var child in attr.Where(a => a.Name == "h4"))
                    {
                        score = WebUtility.HtmlDecode(child.InnerText);
                        score = Regex.Replace(score,
                        @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|<br>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>", "");
                    }
                    foreach (var child in attr.Where(a => a.Name == "p"))
                    {
                        string text = "";
                        text = WebUtility.HtmlDecode(child.InnerHtml);
                        text = Regex.Replace(text,
                        @"\t|\r|<li>|</li>|<ul>|</ul>|<b>|<br>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>", "");
                        
                        if (text == "") cont = false;
                        if (text.Contains("Preview")) cont = false;
                        if (text.Contains("tweet_box")) cont = false;
                        if (text.Contains("youtube_embed"))
                            //cont = false;
                        {
                            string linkText = text;
                            string id = "";
                            int fullLength = linkText.Length;
                            int i = linkText.LastIndexOf(".com/embed/", System.StringComparison.Ordinal);
                            int start = i + 11;
                            id = linkText.Substring(start, fullLength - start);
                            id = id.Substring(0,id.IndexOf('"'));
                            text = "View Video... " + id;
                        }
                        texts.Add(text);
                    }
                    if (texts.Count > 0)
                    {
                        Update update = new Update();
                        update.Text = new List<string>();
                        foreach (string s in texts)
                        {
                            update.Text.Add(s);
                        }
                        update.Time = time;
                        update.Score = score;
                        texts.Clear();
                        if (cont)
                        {
                            updates.Add(update);
                        }
                        score = "";
                        time = "";
                    }
                    
                }

            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
               // Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return updates;
        }

        private UpdateTbl GetId()
        {
            UpdateTbl ut = new UpdateTbl();
            try
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();

                string query = string.Format("SELECT * from db1305421_wpdev.UpdateTbl where Id like '1';");

                using (var cmd = new MySqlCommand(query, _connection))
                {
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            ut.Id = r.GetInt32("UpdateID");
                            ut.Date = DateTime.Parse(r.GetString("Date"));
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
            finally
            {
                _connection.Close();
            }

            return ut;
        }
    }
}
