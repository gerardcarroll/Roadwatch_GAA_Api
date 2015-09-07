using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Route
    {
        public String Title { get; set; }
        public List<Junction> Junctions { get; set; }
    }
}