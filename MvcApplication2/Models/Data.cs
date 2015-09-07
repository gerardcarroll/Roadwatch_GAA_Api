using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Data
    {
        public DateTime Updated { get; set; }
        public Int32 TotalItems { get; set; }
        public Int32 StartIndex { get; set; }
        public Int32 ItemsPerPage { get; set; }
        public List<Items> Items { get; set; }
    }
}