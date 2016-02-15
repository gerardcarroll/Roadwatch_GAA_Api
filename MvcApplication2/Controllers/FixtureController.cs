using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using MvcApplication2.Models;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class FixtureController : ApiController
    {
        public List<Fixture> GetFixtures()
        {
            var fixtures = new List<Fixture>();
            var date = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("yyyyMMdd");
            var json = "";
            var output = "";
            //var url = "http://www.gaa.ie/iphone/feed_cache_json.php?fixturesOnly=Y&includeClubGames=N&owner=1&dateFrom=" + date + "&daysNext=31";

            var url = "http://www.gaa.ie/library/oldfeeds/feed_cache_json?fixturesOnly=Y&includeClubGames=N&owner=1&dateFrom=20160207&daysNext=31";

            try
            {
                using (var client = new WebClient())
                {
                    json = client.DownloadString(url);
                }

                if (json.Contains("null"))
                {
                    return fixtures;
                }
                output = json.Substring(json.IndexOf('['));
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
                    var tv = fixture1.tv.ToString();
                    if (tv == "{}")
                    {
                        tv = "";
                    }
                    var dt = "TBC";
                    if (fixture1.date != "" && fixture1.date.Length >= 10)
                    {
                        dt = fixture1.date.Substring(0, Math.Min(fixture1.date.Length, 10));
                    }

                    var fix = new Fixture
                    {
                        CompName = fixture1.competition_name,
                        Date = dt,
                        ExtraTime = Convert.ToBoolean(fixture1.extratime_playable),
                        ID = Convert.ToInt32(fixture1.unique_id),
                        Ref_County = fixture1.referee_county.ToString(),
                        Ref_Name = refname,
                        Replay = Convert.ToBoolean(fixture1.replay),
                        Team_1 = fixture1.club_1_name,
                        Team_2 = fixture1.club_2_name.ToString(),
                        Tv = tv,
                        Venue = fixture1.venue_name.ToString(),
                        Time = fixture1.time
                    };
                    fixtures.Add(fix);
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }


            return fixtures;
        }
    }
}