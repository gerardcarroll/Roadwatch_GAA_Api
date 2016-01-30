using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using HtmlAgilityPack;
using Microsoft.Ajax.Utilities;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class NewsController : ApiController
    {
        public List<String> GetNews(int id)
        {
            var newsLines = new List<string>();
            if (id == 1)
            {
                var s =
                    "<a href='http://www.windowsphone.com/s?appid=20a95381-7520-42c5-9122-05ecec7d1183'>Click Here To Download From Store</a>";
                newsLines.Add(s);
                return newsLines;
            }
            var text = "";
            var decodedString = "";

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
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>",
                            "");
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
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>",
                            "");
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
            var newsLines = new List<string>();
            if (id == 1)
            {
                var s =
                    "<a href='http://www.windowsphone.com/s?appid=20a95381-7520-42c5-9122-05ecec7d1183'>Click Here To Download From Store</a>";
                newsLines.Add(s);
                return newsLines;
            }
            var text = "";
            var ViewVideo = "";
            var decodedString = "";
            var lastLine = "";
            var web = new HtmlWeb();

            var link = "";
            link = url.Contains("http://cdn.en.gaa.deltatre.nethttp") ? url.Remove(0, 30) : url;

            HtmlDocument doc;
            try
            {
                if (title.Contains("Video:"))
                {
                    doc = web.Load(link);
                    var embedNodeAttributes =
                        doc.DocumentNode.SelectSingleNode("//div[@class='youtube_embed']").FirstChild.Attributes;
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
                    var found = true;
                    doc = web.Load(link);
                    try
                    {
                        embedNodeAttributes =
                            doc.DocumentNode.SelectSingleNode("//div[@class='youtube_embed']").FirstChild.Attributes;
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

                //link = "http://gaa.ie/iphone/get_news_detail_html.php?article_id=" + id;
                doc = web.Load(link);
                var nodes = doc.DocumentNode.SelectSingleNode("//div[@class='news-body']").ChildNodes;
                //var nodes = doc.DocumentNode.SelectNodes("//p");

                var count = nodes.Count();

                if (count == 3)
                {
                    nodes = doc.DocumentNode.SelectNodes("//li");
                    foreach (var node in nodes)
                    {
                        text = WebUtility.HtmlDecode(node.InnerHtml);
                        decodedString = Regex.Replace(text,
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|<em[^>]*>|</em>|<span[^>]*>|</span>|<u>|<u[^>]*>|</u>|<strong>|</strong>|<p>|</p>",
                            "");
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
                    for (var i = 0; i < nodes.Count(); i++)
                    {
                        text = WebUtility.HtmlDecode(nodes[i].InnerHtml);
                        decodedString = Regex.Replace(text.Trim(),
                            @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|<em[^>]*>|</em>|<span[^>]*>|</span>|<u>|<u[^>]*>|</u>|<strong>|</strong>|<p>|</p>|<sup>|</sup>",
                            "");
                        if (decodedString.Contains("<figure>"))
                        {
                            decodedString = Regex.Match(decodedString, "<img.*?data-srcset=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            decodedString = "<img src=\"http://www.gaa.ie" + decodedString + "\"><img>";
                        }
                        if (String.IsNullOrWhiteSpace(decodedString)) continue;
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
                    newsLines.Insert(0, lastLine); //.Add(lastLine);
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