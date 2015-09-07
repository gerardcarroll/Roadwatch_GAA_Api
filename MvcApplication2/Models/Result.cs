using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Result
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
        public String Team_1_Goals { get; set; }
        public String Team_2_Goals { get; set; }
        public String Team_1_Points { get; set; }
        public String Team_2_Points { get; set; }
        public String Team1_Score { get; set; }
        public String Team2_Score { get; set; }
    }
}