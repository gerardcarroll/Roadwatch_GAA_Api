using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class AndroidSummaryBlock
    {
        public String Title { get; set; } //eg. ROADWORKS
        public List<PlaceUpdate> PlaceUpdates { get; set; }
    }

    public class PlaceUpdate
    {
        public String Place { get; set; } //Eg Sligo:
        public String Info { get; set; }  //The road is closed in Sligo Due to .....
    }
}