using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class AndroidNationalSummaryController : ApiController
    {
        public List<AndroidSummaryBlock> GetSummaryBlocks()
        {
            var blocks = new List<AndroidSummaryBlock>();
            var web = new HtmlWeb();
            const string link = "http://www.theaa.ie/AA/AA-Roadwatch.aspx";
            var d = "";
            var placeUpdate = new PlaceUpdate();
            var updateTime = false;
            try
            {
                var doc = web.Load(link);
                var nodes = doc.DocumentNode.SelectNodes("//div[@class='mainTrafficReport']").Descendants();
                var listOfPlaceUpdates = new List<PlaceUpdate>();
                var sb = new AndroidSummaryBlock();
                foreach (var node in nodes.Where(n => n.Name == "#text"))
                {
                    var innerTrimmed = WebUtility.HtmlDecode(node.InnerText).Trim();
                    if (updateTime)
                    {
                        //sb = new AndroidSummaryBlock();
                        //sb.Title = innerTrimmed;
                        //paraList.Add("updated");
                        //sb.Paragraph = paraList;
                        //blocks.Add(sb);
                        //paraList = new List<string>();
                        //sb = new AndroidSummaryBlock();
                        updateTime = false;
                        continue;
                    }
                    if (innerTrimmed == "Last Updated:")
                    {
                        updateTime = true;
                        continue;
                    }
                    if (innerTrimmed == "MAIN TRAFFIC" || innerTrimmed == "*MAIN TRAFFIC*")
                    {
                        if (sb.Title != null)
                        {
                            listOfPlaceUpdates.Add(placeUpdate);
                            placeUpdate = new PlaceUpdate();
                            sb.Title = sb.Title;
                            blocks.Add(sb);
                        }
                        paraList = new List<string>();
                        sb = new AndroidSummaryBlock();
                        sb.Title = "MAIN TRAFFIC";
                    }
                    else if (innerTrimmed == "CITY TRAFFIC" || innerTrimmed == "*CITY TRAFFIC*")
                    {
                        if (sb.Title != null)
                        {
                            paraList.Add(para);
                            para = "";
                            sb.Paragraph = paraList;
                            blocks.Add(sb);
                        }

                        sb = new AndroidSummaryBlock();
                        sb.Title = "CITY TRAFFIC";
                        paraList = new List<string>();
                    }
                    else if (innerTrimmed == "EVENTS" || innerTrimmed == "*EVENTS*")
                    {
                        if (sb.Title != null)
                        {
                            paraList.Add(para);
                            para = "";
                            sb.Paragraph = paraList;
                            blocks.Add(sb);
                        }
                        paraList = new List<string>();
                        sb = new AndroidSummaryBlock();
                        sb.Title = "EVENTS";
                    }
                    else if (innerTrimmed == "ROADWORKS" || innerTrimmed == "*ROADWORKS*")
                    {
                        if (sb.Title != null)
                        {
                            paraList.Add(para);
                            para = "";
                            sb.Paragraph = paraList;
                            blocks.Add(sb);
                        }
                        paraList = new List<string>();
                        sb = new AndroidSummaryBlock();
                        sb.Title = "ROADWORKS";
                    }
                    else if (innerTrimmed == "TRAVEL" || innerTrimmed == "*TRAVEL*")
                    {
                        if (sb.Title != null)
                        {
                            paraList.Add(para);
                            para = "";
                            sb.Paragraph = paraList;
                            blocks.Add(sb);
                        }
                        paraList = new List<string>();
                        sb = new AndroidSummaryBlock();
                        sb.Title = "TRAVEL";
                    }
                    else if (innerTrimmed == "ROAD CONDITIONS" || innerTrimmed == "*ROAD CONDITIONS*")
                    {
                        if (sb.Title != null)
                        {
                            paraList.Add(para);
                            para = "";
                            sb.Paragraph = paraList;
                            blocks.Add(sb);
                        }
                        paraList = new List<string>();
                        sb = new AndroidSummaryBlock();
                        sb.Title = "ROAD CONDITIONS";
                    }
                    else
                    {
                        d = WebUtility.HtmlDecode(node.InnerText);
                        d = d.Trim();
                        if (!d.StartsWith("if ("))
                        {
                            if (d != "&nbsp;" && d != "" && d != "\r\n" && d != "Toggle Map" &&
                                d != "National Summary Report" && !d.StartsWith("Last Updated:"))
                            {
                                if (!d.StartsWith("<!"))
                                {
                                    if (IsAllUpper(d) && d.Length > 1)
                                    {
                                        if (para != "")
                                        {
                                            paraList.Add(para);
                                            para = "";
                                        }
                                        if (!d.EndsWith(":"))
                                        {
                                            d = d + ":";
                                        }
                                        paraList.Add(d);
                                    }
                                    else
                                    {
                                        if (d.StartsWith(":") || d.StartsWith(" :"))
                                        {
                                            var index = d.IndexOf(":", StringComparison.Ordinal);
                                            d = (index < 0) ? d : d.Remove(index, 1);
                                            d = d.Trim();
                                        }
                                        if (d.Length > 1)
                                        {
                                            para += d + " ";
                                        }
                                        else
                                        {
                                            para += d;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                paraList.Add(para);
                sb.Paragraph = paraList;
                blocks.Add(sb);
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            return blocks;
        }

        private bool IsAllUpper(string input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                    return false;
            }
            return true;
        }
    }
}
