using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MvcApplication2.Models;
using Newtonsoft.Json;

namespace MvcApplication2.Controllers
{
    public class TravelTimeController : ApiController
    {
        public List<TravelTime> GetTimes()
        {
            var times = new List<TravelTime>();

            var obj = JsonConvert.DeserializeObject<List<Route>>(GetString());

            return times;
        }

        private string GetString()
        {
            using (var client = new WebClient())
            {
                var json =
                    client.DownloadString(
                        "https://dataproxy.mtcc.ie/v1.5/api/traveltimes?callback=populateTableDate&_=1405630580577");
                json = json.Replace("populateTableDate(", "");
                json = json.TrimEnd(')');
                json = json.Insert(0, "[");
                json = json.Insert(json.Count(), "]");
                return json;
            }
        }
    }
}