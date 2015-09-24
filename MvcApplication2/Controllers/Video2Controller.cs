using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MvcApplication2.Models;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class Video2Controller : ApiController
    {
        public List<Video2> GetVideos()
        {
            var videos = new List<Video2>();
            var json = "";

            var channelId = "UCCsDOY9UaBpPnv60mxX6dQA";
            var apiKey = "AIzaSyBVhdowoULpVYbhUMELZ3lHeKPvGWpNkcE";

            try
            {
                YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = apiKey });

                var searchListRequest = yt.Search.List("snippet");
                searchListRequest.PublishedAfter = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0, 0));
                searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                searchListRequest.ChannelId = channelId;
                searchListRequest.MaxResults = 30;
                var searchListResult = searchListRequest.Execute();

                videos.AddRange(searchListResult.Items.Select(item => new Video2()
                {
                    Id = item.Id.VideoId,
                    Description = item.Snippet.Description,
                    Thumbnail = item.Snippet.Thumbnails.High.Url,
                    Title = item.Snippet.Title
                }));

                //using (var client = new WebClient())
                //{
                //    json =
                //        client.DownloadString(
                //            "http://gdata.youtube.com/feeds/api/videos?author=officialgaa&max-results=20&v=2&alt=jsonc&orderby=published");
                //}

                //var obj = JsonConvert.DeserializeObject<Youtube>(json);
                //foreach (var vid in obj.Data.Items)
                //{
                //    if (vid.Id != "UKY3scPIMd8")
                //    {
                //        var video2 = new Video2
                //        {
                //            Id = vid.Id,
                //            Description = Encode(vid.Description),
                //            Duration = vid.Duration.ToString(),
                //            Thumbnail = vid.Thumbnail.sqDefault,
                //            Title = Encode(vid.Title)
                //        };
                //        videos.Add(video2);
                //    }
                //}
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
            var bytes = Encoding.Default.GetBytes(p);
            p = Encoding.UTF8.GetString(bytes);
            return p;
        }
    }
}