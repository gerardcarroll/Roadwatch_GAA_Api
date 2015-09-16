using System;

namespace MvcApplication2.Models
{
    public class Article1
    {
        public int ID { get; set; }
        public String title { get; set; }
        public String content_text_id { get; set; }
        public String content_image_id { get; set; }
        public DateTime upload_date { get; set; }
        public String filename { get; set; }
        public String thumbnail { get; set; }
        public String url { get; set; }
        public String code { get; set; }
        public bool New { get; set; }
    }
}