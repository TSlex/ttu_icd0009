using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace Domain
{
    public class Post : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string PostTitle { get; set; } = default!;

        public Guid? PostImageId { get; set; }
        public Image? PostImage { get; set; }

        [MaxLength(500)] public string? PostDescription { get; set; }

        public DateTime PostPublicationDateTime { get; set; } = default!;

        public int PostFavoritesCount { get; set; }
        public int PostCommentsCount { get; set; }

        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
    }
}