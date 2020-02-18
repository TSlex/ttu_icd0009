using System;
using System.Collections.Generic;

namespace Domain
{
    public class Post
    {
        public string PostId { get; set; }
        
        public string PostImageUrl { get; set; }
        public string PostTitle { get; set; }
        public string PostDescription { get; set; }
        
        public DateTime PostPublicationDateTime { get; set; }

        public int PostFavoritesCount { get; set; }
        public int PostCommentsCount { get; set; }
        
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}