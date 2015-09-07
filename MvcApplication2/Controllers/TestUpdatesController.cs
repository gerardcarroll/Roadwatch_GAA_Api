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

namespace MvcApplication2.Controllers
{
    public class TestUpdatesController : ApiController
    {
        public List<Update> GetUpdates(string url)
        {
            List<Update> updates = new List<Update>();
            UpdateTbl ut = new UpdateTbl();
            
            url = "http://www.gaa.ie/modules/live_match.php?id=10830";
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
                foreach (var node in nodes)
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
                        if (text.Contains("youtube_embed")) cont = false;
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
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return updates;
        }
    }
}
