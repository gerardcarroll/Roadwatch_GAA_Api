using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Fixture
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
        public String CompName { get; set; }
        public string Ref_Name { get; set; }
        public string Ref_County { get; set; }
        public string Team_1 { get; set; }
        public string Team_2 { get; set; }
        public bool ExtraTime { get; set; }
        public bool Replay { get; set; }
        public String Tv { get; set; }
        //public string Team1_Crest { get; set; }
        //public string Team2_Crest { get; set; }
    }
}