using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using MvcApplication2.Models;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class Video2Controller : ApiController
    {
        public List<Video2> GetVideos()
        {
            List<Video2> videos = new List<Video2>();
            string json = "";

            try
            {
                using (var client = new WebClient())
                {
                    json = client.DownloadString("http://gdata.youtube.com/feeds/api/videos?author=officialgaa&max-results=20&v=2&alt=jsonc&orderby=published");
                }

                var obj = JsonConvert.DeserializeObject<Youtube>(json);
                foreach (var vid in obj.Data.Items)
                {
                    if (vid.Id != "UKY3scPIMd8")
                    {
                        Video2 video2 = new Video2
                        {
                            Id = vid.Id,
                            Description = Encode(vid.Description),
                            Duration = vid.Duration.ToString(),
                            Thumbnail = vid.Thumbnail.sqDefault,
                            Title = Encode(vid.Title)
                        };
                        videos.Add(video2);
                    }
                }
                
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return videos;
        }

        private string Encode(string p)
        {
            byte[] bytes = Encoding.Default.GetBytes(p);
            p = Encoding.UTF8.GetString(bytes);
            return p;
        }
    }
}
