using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using HtmlAgilityPack;
using MvcApplication2.Models;
using System.Xml.Linq;

namespace MvcApplication2.Controllers
{
    public class VideoController : ApiController
    {
        public List<Video> GetVideo()
        {
            var videos = new List<Video>();

            var web = new HtmlWeb();
            const string link = "http://gaa.mobilerider.com/api/mrss-m4v.php?limit=15";

            try
            {
                var doc = web.Load(link);

                var nodes = doc.DocumentNode.SelectNodes("//url");

                videos.AddRange(nodes.Select(node => new Video
                                                     {
                                                         Thumbnail = node.ChildNodes[3].ChildNodes[3].InnerHtml,
                                                         Title = node.ChildNodes[3].ChildNodes[5].InnerHtml,
                                                         Description = node.ChildNodes[3].ChildNodes[7].InnerHtml,
                                                         Duration = node.ChildNodes[3].ChildNodes[13].InnerHtml,
                                                         M4V = node.ChildNodes[3].ChildNodes[17].InnerHtml
                                                     }));
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return videos;
        }
    }
}
