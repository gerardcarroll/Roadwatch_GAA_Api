using System.Xml.Serialization;

namespace MvcApplication2.Models
{
    /// <remarks />
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Articles
    {
        /// <remarks />
        [XmlElement("Article")]
        public ArticlesArticle[] Article { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class ArticlesArticle
    {
        /// <remarks />
        public string title { get; set; }

        /// <remarks />
        public ushort content_text_id { get; set; }

        /// <remarks />
        public ushort content_image_id { get; set; }

        /// <remarks />
        public string upload_date { get; set; }

        /// <remarks />
        public string filename { get; set; }

        /// <remarks />
        public string thumbnail { get; set; }

        /// <remarks />
        public string @abstract { get; set; }

        /// <remarks />
        public string url { get; set; }

        /// <remarks />
        public string code { get; set; }

        /// <remarks />
        [XmlAttribute]
        public ushort id { get; set; }
    }
}