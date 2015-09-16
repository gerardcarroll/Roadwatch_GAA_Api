using System;

namespace MvcApplication2.Models
{
    public class TravelTime
    {
        public String Title { get; set; }
        public Int32 CurrentTime { get; set; }
        public Int32 Distance { get; set; }
        public Int32 FreeFlowTime { get; set; }
        public String From { get; set; }
        public String To { get; set; }
        public Char Status { get; set; }
    }
}