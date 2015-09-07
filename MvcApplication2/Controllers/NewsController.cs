using System.Collections;
using System.Diagnostics;
using System.Reflection;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class NewsController : ApiController
    {
        public List<String> GetNews(int id)
        {
            List<String> newsLines = new List<string>();
            if (id == 1)
            {
                string s = "<a href='http://www.windowsphone.com/s?appid=20a95381-7520-42c5-9122-05ecec7d1183'>Click Here To Download From Store</a>";
                newsLines.Add(s);
                return newsLines;
            }
            string text = "";
            string decodedString = "";
           
            var web = new HtmlWeb();
            HtmlDocument doc;
            try
            {
                var link = "http://gaa.ie/iphone/get_news_detail_html.php?article_id=" + id;
                doc = web.Load(link);
                var nodes = doc.DocumentNode.SelectNodes("//p");

                var count = nodes.Count();

                if (count == 3)
                {
                    nodes = doc.DocumentNode.SelectNodes("//li");
                    foreach (var node in nodes)
                    {
                        text = WebUtility.HtmlDecode(node.InnerHtml);
                        decodedString = Regex.Replace(text,
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>", "");
                        if (decodedString.Contains("[highlight")) continue;
                        if (decodedString.Contains("View tweet")) continue;
                        if (decodedString.Contains("<br>"))
                        {
                            decodedString = decodedString.Replace("<br>", "\r\n");
                        }
                        newsLines.Add(decodedString);
                    }
                }
                else
                {
                    for (var i = 3; i < nodes.Count(); i++)
                    {
                        text = WebUtility.HtmlDecode(nodes[i].InnerHtml);
                        decodedString = Regex.Replace(text,
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>", "");
                        if (decodedString.Contains("[highlight")) continue;
                        if (decodedString.Contains("View tweet")) continue;
                        if (decodedString.Contains("<br>"))
                        {
                            decodedString = decodedString.Replace("<br>", "\r\n");
                        }
                        newsLines.Add(decodedString);
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return newsLines;
        }

        public List<String> GetNews(int id, string url, string title)
        {
            List<String> newsLines = new List<string>();
            if (id == 1)
            {
                string s = "<a href='http://www.windowsphone.com/s?appid=20a95381-7520-42c5-9122-05ecec7d1183'>Click Here To Download From Store</a>";
                newsLines.Add(s);
                return newsLines;
            }
            string text = "";
            string ViewVideo = "";
            string decodedString = "";
            string lastLine = "";
            var web = new HtmlWeb();
            string link = url;
            HtmlDocument doc;
            try
            {
                if (title.Contains("Video:"))
                {
                    doc = web.Load(link);
                    var embedNodeAttributes = doc.DocumentNode.SelectSingleNode("//div[@class='youtube_embed']").FirstChild.Attributes;
                    foreach (var attribute in embedNodeAttributes)
                    {
                        if (attribute.Name == "src")
                        {
                            lastLine = "<a href='https:" + attribute.Value + "' >View Video</a>";
                        }
                    }
                }
                else
                {
                    HtmlAttributeCollection embedNodeAttributes = null;
                    bool found = true;
                    doc = web.Load(url);
                    try
                    {
                        embedNodeAttributes = doc.DocumentNode.SelectSingleNode("//div[@class='youtube_embed']").FirstChild.Attributes;
                        
                    }
                    catch (Exception ex)
                    {
                        found = false;
                    }
                    if (found)
                    {
                        if (embedNodeAttributes != null)
                        {
                            foreach (var attribute in embedNodeAttributes)
                            {
                                if (attribute.Name == "src")
                                {
                                    lastLine = "<a href='https:" + attribute.Value + "' >View Video</a>";
                                }
                            }
                        }
                    }
                    
                    
                }

                link = "http://gaa.ie/iphone/get_news_detail_html.php?article_id=" + id;
                doc = web.Load(link);
                var nodes = doc.DocumentNode.SelectNodes("//p");

                var count = nodes.Count();

                if (count == 3)
                {
                    nodes = doc.DocumentNode.SelectNodes("//li");
                    foreach (var node in nodes)
                    {
                        text = WebUtility.HtmlDecode(node.InnerHtml);
                        decodedString = Regex.Replace(text,
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|<em[^>]*>|</em>|<span[^>]*>|</span>|<u>|<u[^>]*>|</u>|<strong>|</strong>|<p>|</p>", "");
                        if (decodedString.Contains("[highlight")) continue;
                        if (decodedString.Contains("View tweet")) continue;
                        if (decodedString.Contains("<br>"))
                        {
                            decodedString = decodedString.Replace("<br>", "\r\n");
                        }
                        newsLines.Add(decodedString);
                    }
                }
                else
                {
                    for (var i = 3; i < nodes.Count(); i++)
                    {
                        text = WebUtility.HtmlDecode(nodes[i].InnerHtml);
                        decodedString = Regex.Replace(text,
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|<em[^>]*>|</em>|<span[^>]*>|</span>|<u>|<u[^>]*>|</u>|<strong>|</strong>|<p>|</p>", "");
                        if (decodedString.Contains("[highlight")) continue;
                        if (decodedString.Contains("View tweet")) continue;
                        if (decodedString.Contains("<br>"))
                        {
                            decodedString = decodedString.Replace("<br>", "\r\n");
                        }
                        if (decodedString.Contains("View video") && ViewVideo != "View Video")
                        {
                            newsLines.Add(decodedString);
                            ViewVideo = "View Video";
                        }
                        else if (!decodedString.Contains("View video"))
                        {
                            newsLines.Add(decodedString);
                            ViewVideo = "";
                        }
                        else
                        {
                            ViewVideo = "";
                        }
                        
                    }
                }

                if (lastLine != "")
                {
                    newsLines.Insert(0, lastLine);//.Add(lastLine);
                }

            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return newsLines;
        }
    }
}
