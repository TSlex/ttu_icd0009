using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class Post : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)]
        public string PostTitle { get; set; } = default!;

        [MaxLength(300)]
//        public string? PostImageUrl { get; set; }
        
        public Guid? PostImageId { get; set; }
        public Image? PostImage { get; set; }

        [MaxLength(500)]
        public string? PostDescription { get; set; }
        
        public DateTime PostPublicationDateTime { get; set; } = DateTime.UtcNow;
        
        public int PostFavoritesCount { get; set; } = 0;
        
        public int PostCommentsCount { get; set; } = 0;
        
        public Guid ProfileId { get; set; }
        
        public Profile? Profile { get; set; }
        
        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Favorite>? Favorites { get; set; }
        
        public bool IsUserFavorite { get; set; }
    }
}