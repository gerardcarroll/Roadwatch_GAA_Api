using System;

namespace MvcApplication2.Models
{
    public class Junction
    {
        public String Status { get; set; }
        public Int32 Distance { get; set; }
        public String From_Name { get; set; }
        public Int32 Current_Travel_Time { get; set; }
        public String To_Name { get; set; }
        public Int32 Free_Flow_Travel_Time { get; set; }
    }
}