using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{

    public class Tag
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Text
    {
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string text { get; set; }
        public string Html { get; set; }
        public string ContentResourceId { get; set; }
    }

    public class Post
    {
        public string HTML { get; set; }
        public List<object> PostData { get; set; }
        public int Id { get; set; }
        public int Order { get; set; }
        public string PostDate { get; set; }
        public string LastUpdateDate { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public bool Sticky { get; set; }
        public int AuthorID { get; set; }
        public string Author { get; set; }
        public object SourceLang { get; set; }
        public string AuthorAvatar { get; set; }
        public string AuthorRole { get; set; }
        public string Image { get; set; }
        public string DetailImage { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public object PostType { get; set; }
        public object PostSubType { get; set; }
        public string PostIconPath { get; set; }
        public object Copyright { get; set; }
        public string ContentAuthor { get; set; }
        public string ContentAuthorName { get; set; }
        public string ContentAuthorAvatar { get; set; }
        public string OriginalContentDateTime { get; set; }
        public bool ShowAvatarOnPost { get; set; }
        public string ExternalResourceId { get; set; }
        public string ExternalResourceType { get; set; }
        public string ContentResourceId { get; set; }
        public List<Text> Text { get; set; }
        public List<object> Tags { get; set; }
    }

    public class LiveFeedData
    {
        public int Id { get; set; }
        public int LiveBlogTemplateId { get; set; }
        public string LiveBlogDescription { get; set; }
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public int CommentsMode { get; set; }
        public int TypeId { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string ScheduleDate { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatar { get; set; }
        public string AuthorBiography { get; set; }
        public string AuthorRole { get; set; }
        public int TotalPosts { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Post> Posts { get; set; }
    }

}