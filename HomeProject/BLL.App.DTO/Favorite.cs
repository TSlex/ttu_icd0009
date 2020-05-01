using System;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Favorite: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public ProfileFull? Profile { get; set; }
        
        public Guid PostId { get; set; } = default!;
        public Post? Post { get; set; }
        
        //what content user actually likes
        public string PostTitle { get; set; }
        public string? PostImageUrl { get; set; }
        public string? PostDescription { get; set; }
    }
}