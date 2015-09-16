using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public class Attributes
    {
        public string id { get; set; }
    }

    public class County1
    {
    }

    public class County2
    {
    }

    public class Sections
    {
        public List<string> section_id { get; set; }
    }

    public class Article
    {
        public Attributes attributes { get; set; }
        public string title { get; set; }
        public County1 county1 { get; set; }
        public County2 county2 { get; set; }
        public string newsid { get; set; }
        public Sections sections { get; set; }
        public string content_text_id { get; set; }
        public string content_image_id { get; set; }
        public string content_video { get; set; }
        public string upload_date { get; set; }
        public string filename { get; set; }
        public string thumbnail { get; set; }
        public string url { get; set; }
    }

    public partial class RootObject
    {
        public List<Article> Article { get; set; }
    }
}