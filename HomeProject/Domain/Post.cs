using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Post : DomainEntity
    {
        [MaxLength(100)]
        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.Domain.Post))]
        public string PostTitle { get; set; } = default!;

        [MaxLength(300)]
        [Display(Name = nameof(PostImageUrl), ResourceType = typeof(Resourses.Domain.Post))]
        public string? PostImageUrl { get; set; }

        [MaxLength(500)]
        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.Domain.Post))]
        public string? PostDescription { get; set; }

        [Display(Name = nameof(PostPublicationDateTime), ResourceType = typeof(Resourses.Domain.Post))]
        public DateTime PostPublicationDateTime { get; set; } = DateTime.Now;

        [Display(Name = nameof(PostFavoritesCount), ResourceType = typeof(Resourses.Domain.Post))]
        public int PostFavoritesCount { get; set; } = 0;

        [Display(Name = nameof(PostCommentsCount), ResourceType = typeof(Resourses.Domain.Post))]
        public int PostCommentsCount { get; set; } = 0;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.Domain.Post))]
        public Guid ProfileId { get; set; }

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.Domain.Post))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(Comments), ResourceType = typeof(Resourses.Domain.Post))]
        public ICollection<Comment>? Comments { get; set; }

        [Display(Name = nameof(Favorites), ResourceType = typeof(Resourses.Domain.Post))]
        public ICollection<Favorite>? Favorites { get; set; }
    }
}