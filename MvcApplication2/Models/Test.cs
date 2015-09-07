using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Place
    {
        public String Area { get; set; }
        public Int32 ID { get; set; }
        public Int32 IncidentTypeID { get; set; }
        public Double Latitude { get; set; }
        public String Location { get; set; }
        public Double Longitude { get; set; }
        public String Report { get; set; }
        public String Title { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Int32 ZoomLevel { get; set; }
    }
}