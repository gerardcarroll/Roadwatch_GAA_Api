using System.Web.Http;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class RoadwatchErrorController : ApiController
    {
        private const string S = "this";
        // GET api/error
        public string Get(string method, string exception, string full_Ex, string model)
        {
            Database.InsertRoadwatchPhoneErrorToDb(method, exception, full_Ex, model);

            return S;
        }
    }
}