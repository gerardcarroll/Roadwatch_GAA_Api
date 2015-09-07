using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Web;
using System.Web.Mvc;
using ImageResizer;
using ImageResizer.Configuration;
using MvcApplication2.Models;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class LiveTileController : Controller
    {
        //
        // GET: /LiveTile/

        public FileResult Icon()
        {
            return Resize(336, 336);
        }

        public FileResult LargeIcon()
        {
            return Resize(336,691);
        }
        private FileResult Resize(int height, int width)
        {
            Article1 article1 = new Article1();

            string json = "";
            
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

                var q = (from a in articless
                    select a).OrderByDescending(a => a.upload_date).FirstOrDefault();
                
                Article art = q;

                article1.title = WebUtility.HtmlDecode(art.title);
                article1.filename = art.filename.Replace("http:", "");

                var uri = "http:" + article1.filename;
                var rsSettings = new ResizeSettings() { Format = "jpg", MaxHeight = height, MaxWidth = width };
                rsSettings.Anchor = ContentAlignment.TopCenter;
                rsSettings.Mode = FitMode.Crop;
                rsSettings.Scale = ScaleMode.Both;
                Config c = Config.Current;

                var wc = new WebClient
                {
                    CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
                };
                var dt = wc.DownloadData(uri);
                using (var ms = new MemoryStream(dt))
                {
                    var mStream = new MemoryStream();
                    c.CurrentImageBuilder.Build(ms, mStream, rsSettings);
                    mStream.Seek(0, SeekOrigin.Begin);
                    return new FileStreamResult(mStream, "image/jpeg");
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return null;

        }

        public String Heading()
        {
            Article1 article1 = new Article1();

            string json = "";
            
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

                var q = (from a in articless
                         select a).OrderByDescending(a => a.upload_date).FirstOrDefault();

                Article art = q;

                article1.title = WebUtility.HtmlDecode(art.title);
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            return article1.title;
        }


    }
}
