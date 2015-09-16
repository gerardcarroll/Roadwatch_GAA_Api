using System;

namespace MvcApplication2.Models
{
    public class Items
    {
        public String Id { get; set; }
        public String Description { get; set; }
        public String Title { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public Int32 Duration { get; set; }
    }
}