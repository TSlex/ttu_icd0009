using System;
using DAL.Base;

namespace DAL.App.DTO
{
    public class Favorite: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid PostId { get; set; } = default!;
        public Post? Post { get; set; }
        
        //what content user actually likes
        public string PostTitle { get; set; } = default!;
        public string? PostImageUrl { get; set; }
        public string? PostDescription { get; set; }
    }
}