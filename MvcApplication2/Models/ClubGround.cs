using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class ClubGround
    {
        public Int32 Id { get; set; }
        public String County { get; set; }
        public String Club_Ground { get; set; }
        public String Club_Ground_2 { get; set; }
        public String Longitude { get; set; }
        public String Latitude { get; set; }
        public String Colours { get; set; }
        public String Website { get; set; }
        public String Facebook { get; set; }
        public String Twitter { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
    }
}