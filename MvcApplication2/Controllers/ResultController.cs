using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using MvcApplication2.Models;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class ResultController : ApiController
    {
        public List<Result> GetResults()
        {
            var results = new List<Result>();

            var json = "";
            try
            {
                //string xmlUrl =
                //    "http://www.gaa.ie/iphone/feed_cache.php?includeClubGames=N&owner=1&reverseDateOrder=Y&dateFrom=" +
                //    DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0)).ToString("yyyyMMdd") + "&dateTo=" +
                //    DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                //XDocument resXml = XDocument.Load(xmlUrl);

                //var xElement = resXml.Element("fixtures");
                //if (xElement != null)
                //    results = (from fixture1 in xElement.Element("fixture")
                //               select new Result
                //               {
                //                   CompName = fixture1.competition_name,
                //                   Date = fixture1.date,
                //                   ExtraTime = Convert.ToBoolean(fixture1.extratime_playable),
                //                   ID = Convert.ToInt32(fixture1.unique_id),
                //                   Ref_County = fixture1.referee_county.ToString(),
                //                   Ref_Name = refname,
                //                   Replay = Convert.ToBoolean(fixture1.replay),
                //                   Team_1 = fixture1.club_1_name,
                //                   Team_2 = fixture1.club_2_name,
                //                   Venue = fixture1.venue_name,
                //                   Time = fixture1.time,
                //                   Team1_Score = fixture1.team_1_goals + "-" + fixture1.team_1_points,
                //                   Team2_Score = fixture1.team_2_goals + "-" + fixture1.team_2_points,
                //                   Team_1_Goals = fixture1.team_1_goals,
                //                   Team_1_Points = fixture1.team_1_points,
                //                   Team_2_Goals = fixture1.team_2_goals,
                //                   Team_2_Points = fixture1.team_2_points
                //               }).ToList();

                var url = "http://www.gaa.ie/library/oldfeeds/feed_cache_json?fixturesOnly=Y&includeClubGames=N&owner=1&dateFrom=20160207&daysNext=31";
                using (var client = new WebClient())
                {
                    json =
                        client.DownloadString(
                            "http://www.gaa.ie/iphone/feed_cache_json.php?resultsOnly=Y&includeClubGames=N&owner=1&daysPrevious=31&reverseDateOrder=Y");
                }
                var output = json.Substring(json.IndexOf('['));
                output = output.TrimEnd('}');
                output = output.TrimEnd('}');
                var matchTrack = JsonConvert.DeserializeObject<List<Fixture1>>(output);

                foreach (var fixture1 in matchTrack)
                {
                    var refname = fixture1.referee_forename + " " + fixture1.referee_surname;
                    if (refname == "{} {}")
                    {
                        refname = "";
                        fixture1.referee_county = "";
                    }
                    var res = new Result
                    {
                        CompName = fixture1.competition_name,
                        Date = fixture1.date,
                        ExtraTime = Convert.ToBoolean(fixture1.extratime_playable),
                        ID = Convert.ToInt32(fixture1.unique_id),
                        Ref_County = fixture1.referee_county.ToString(),
                        Ref_Name = refname,
                        Replay = Convert.ToBoolean(fixture1.replay),
                        Team_1 = fixture1.club_1_name,
                        Team_2 = fixture1.club_2_name.ToString(),
                        Venue = fixture1.venue_name.ToString(),
                        Time = fixture1.time,
                        Team1_Score = fixture1.team_1_goals + "-" + fixture1.team_1_points,
                        Team2_Score = fixture1.team_2_goals + "-" + fixture1.team_2_points,
                        Team_1_Goals = fixture1.team_1_goals,
                        Team_1_Points = fixture1.team_1_points,
                        Team_2_Goals = fixture1.team_2_goals,
                        Team_2_Points = fixture1.team_2_points
                    };
                    results.Add(res);
                }
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return results;
        }
    }
}