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
    public class NationalSummaryController : ApiController
    {
        public List<SummaryBlock> GetSummaryBlocks()
        {
            List<SummaryBlock> blocks = new List<SummaryBlock>();

            string decodedString = "";
            string text = "";
            var web = new HtmlWeb();
            const string link = "http://www.theaa.ie/AA/AA-Roadwatch.aspx";
            
            try
            {
                var doc = web.Load(link);
                //string lastUpdated = doc.DocumentNode.SelectSingleNode("//div[@class='lastUpdated']").InnerText;
                var nodes = doc.DocumentNode.SelectNodes("//div[@class='wideGreyWidget']");
                //var nodes = doc.DocumentNode.SelectNodes("//div[@class='mainTrafficReport']").Descendants();
                //List<String> paraList = new List<String>();
                //SummaryBlock sb = new SummaryBlock();
                foreach (var node in nodes)
                {
                    SummaryBlock sb = new SummaryBlock();
                    //title
                    text = WebUtility.HtmlDecode(node.ChildNodes[1].InnerText);
                    decodedString = Regex.Replace(text,
                        @"\t|\n|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>", "");
                    sb.Title = decodedString;

                    List<String> paraList = new List<String>();
                    //para
                    string d = "";
                    if (node.ChildNodes[3].ChildNodes[1].InnerText != "&nbsp;")
                    {
                        d = WebUtility.HtmlDecode(node.ChildNodes[3].ChildNodes[1].InnerText);
                    }
                    else
                    {
                        d = WebUtility.HtmlDecode(node.ChildNodes[3].InnerText);
                    }

                    if (d != "")
                    {
                        d = d.Replace("\n\n\n", "\n");
                        d = d.Trim();
                        decodedString = Regex.Replace(d,
                            @"\t|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>",
                            "");
                        paraList.Add(decodedString);
                    }
                    else
                    {
                        d = WebUtility.HtmlDecode(node.ChildNodes[3].InnerText);
                        d = d.Replace("\n\n\n", "\n");
                        d = d.Trim();
                        decodedString = Regex.Replace(d,
                            @"\t|\r|<li>|</li>|<ul>|</ul>|<b>|</b>|<em>|</em>|<u>|</u>|<strong>|</strong>|<p>|</p>",
                            "");
                        paraList.Add(decodedString);
                    }

                    sb.Paragraph = paraList;
                    blocks.Add(sb);
                }

            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            return blocks;
        }
        
    }
}
