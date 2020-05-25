using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Post : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(100)]
        public string PostTitle { get; set; } = default!;

        [Display(Name = nameof(PostImageId), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public Guid? PostImageId { get; set; }

        [Display(Name = nameof(PostImage), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public Image? PostImage { get; set; }

        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(500)]
        public string? PostDescription { get; set; }

        [Display(Name = nameof(PostPublicationDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public DateTime PostPublicationDateTime { get; set; } = DateTime.Now;

        [Display(Name = nameof(PostFavoritesCount), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public int PostFavoritesCount { get; set; } = 0;

        [Display(Name = nameof(PostCommentsCount), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public int PostCommentsCount { get; set; } = 0;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public Guid ProfileId { get; set; }

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(Comments), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public ICollection<Comment>? Comments { get; set; }

        [Display(Name = nameof(Favorites), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public ICollection<Favorite>? Favorites { get; set; }

        [Display(Name = nameof(IsUserFavorite), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public bool IsUserFavorite { get; set; }

        [Display(Name = nameof(ReturnUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? ReturnUrl { get; set; }
    }
}