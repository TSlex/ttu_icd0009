using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using DomainEntityBaseMetadata = BLL.App.DTO.Base.DomainEntityBaseMetadata;

namespace BLL.App.DTO
{
    public class Favorite : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(PostId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid PostId { get; set; } = default!;

        [Display(Name = nameof(Post), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public Post? Post { get; set; }

        //what content user actually likes
        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public string? PostTitle { get; set; }

        [Display(Name = nameof(PostImageId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public Guid? PostImageId { get; set; }

        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public string? PostDescription { get; set; }
    }
}