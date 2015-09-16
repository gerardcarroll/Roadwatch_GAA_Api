using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using MvcApplication2.Models;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class GAATaskController : ApiController
    {
        public Article1 GetTaskArticle()
        {
            Article1 article = null;
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

                var topArticle = (from a in articless
                    select a).First();

                topArticle.filename = topArticle.filename.Replace("http:", "");
                foreach (var sec in topArticle.sections.section_id)
                {
                    if (sec == "2")
                    {
                        newCode = "Football";
                        break;
                    }
                    if (sec == "3")
                    {
                        newCode = "Hurling";
                        break;
                    }
                    newCode = "General";
                }

                article = new Article1
                {
                    content_image_id = topArticle.content_image_id,
                    content_text_id = topArticle.content_text_id,
                    filename = topArticle.filename,
                    ID = Convert.ToInt32(topArticle.newsid),
                    thumbnail = topArticle.thumbnail,
                    title = WebUtility.HtmlDecode(topArticle.title),
                    upload_date = DateTime.Parse(topArticle.upload_date),
                    url = topArticle.url,
                    code = newCode
                };
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return article;
        }
    }
}