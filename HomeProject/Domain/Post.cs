using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Post: DomainEntityMetadata
    {
        [MaxLength(100)] public string PostTitle { get; set; } = default!;
        [MaxLength(300)] public string? PostImageUrl { get; set; }
        [MaxLength(500)] public string? PostDescription { get; set; }

        public DateTime PostPublicationDateTime { get; set; } = DateTime.Now;

        public int PostFavoritesCount { get; set; } = 0;
        public int PostCommentsCount { get; set; } = 0;

        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
    }
}