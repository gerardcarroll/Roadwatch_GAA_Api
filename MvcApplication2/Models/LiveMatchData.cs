using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class LiveMatchData
    {
        public int BlogId { get; set; }
        public string SentinelPath { get; set; }
        public string FeedPath { get; set; }
        public string MatchDataPath { get; set; }
        public int PollingInterval { get; set; }
        public int DisplayPageSize { get; set; }
    }
}