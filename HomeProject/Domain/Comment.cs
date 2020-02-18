using System;

namespace Domain
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentValue{ get; set; }
        public DateTime CommentDateTime { get; set; }
        
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}