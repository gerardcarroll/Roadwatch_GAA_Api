using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Venue
    {
        public int ID { get; set; }
        public String County { get; set; }
        public String Club { get; set; }
        public String Latitude { get; set; }
        public String Longitude { get; set; }
        public String Name { get; set; }
        public String Town { get; set; }

    }
}