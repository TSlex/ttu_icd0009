using System;

namespace Domain
{
    public class Post
    {
        public int PostId { get; set; }
        
        public string PostImageUrl { get; set; }
        public string PostTitle { get; set; }
        public string PostDescription { get; set; }
        
        public DateTime PostPublicationDateTime { get; set; }

        public int PostFavoritesCount { get; set; }
        public int PostCommentsCount { get; set; }
    }
}