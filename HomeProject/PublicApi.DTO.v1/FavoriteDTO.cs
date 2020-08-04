using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FavoriteProfileDTO
    {
        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }
    }

    public class FavoriteAdminDTO : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(PostId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid PostId { get; set; } = default!;

        //what content user actually likes
        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public string? PostTitle { get; set; }

        [Display(Name = nameof(PostImageId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public Guid? PostImageId { get; set; }

        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public string? PostDescription { get; set; }
    }
}