using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Articles
    {

        private ArticlesArticle[] articleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Article")]
        public ArticlesArticle[] Article
        {
            get
            {
                return this.articleField;
            }
            set
            {
                this.articleField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ArticlesArticle
    {

        private string titleField;

        private ushort content_text_idField;

        private ushort content_image_idField;

        private string upload_dateField;

        private string filenameField;

        private string thumbnailField;

        private string abstractField;

        private string urlField;

        private string codeField;

        private ushort idField;

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public ushort content_text_id
        {
            get
            {
                return this.content_text_idField;
            }
            set
            {
                this.content_text_idField = value;
            }
        }

        /// <remarks/>
        public ushort content_image_id
        {
            get
            {
                return this.content_image_idField;
            }
            set
            {
                this.content_image_idField = value;
            }
        }

        /// <remarks/>
        public string upload_date
        {
            get
            {
                return this.upload_dateField;
            }
            set
            {
                this.upload_dateField = value;
            }
        }

        /// <remarks/>
        public string filename
        {
            get
            {
                return this.filenameField;
            }
            set
            {
                this.filenameField = value;
            }
        }

        /// <remarks/>
        public string thumbnail
        {
            get
            {
                return this.thumbnailField;
            }
            set
            {
                this.thumbnailField = value;
            }
        }

        /// <remarks/>
        public string @abstract
        {
            get
            {
                return this.abstractField;
            }
            set
            {
                this.abstractField = value;
            }
        }

        /// <remarks/>
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
}