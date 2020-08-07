using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace PublicApi.DTO.v1
{
    public class PostGetDTO
    {
        public Guid Id { get; set; } = default!;

        public string PostTitle { get; set; } = default!;
        public string? PostDescription { get; set; }

        public string ProfileUsername { get; set; } = default!;

        public Guid? PostImageId { get; set; }

        public DateTime PostPublicationDateTime { get; set; } = default!;

        public int PostFavoritesCount { get; set; }
        public int PostCommentsCount { get; set; }

        public bool IsFavorite { get; set; }
    }

    public class PostCreateDTO
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid Id { get; set; } = default!;

        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string PostTitle { get; set; } = default!;

        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? PostDescription { get; set; }

        [Display(Name = nameof(PostImageId), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public Guid? PostImageId { get; set; }
    }

    public class PostEditDTO
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid Id { get; set; } = default!;

        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string PostTitle { get; set; } = default!;

        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? PostDescription { get; set; }
    }

    public class PostAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string PostTitle { get; set; } = default!;

        [Display(Name = nameof(PostImageId), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public Guid? PostImageId { get; set; }

        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? PostDescription { get; set; }

        [Display(Name = nameof(PostPublicationDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public DateTime PostPublicationDateTime { get; set; } = DateTime.UtcNow;

        [Display(Name = nameof(PostFavoritesCount), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public int PostFavoritesCount { get; set; } = 0;

        [Display(Name = nameof(PostCommentsCount), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        public int PostCommentsCount { get; set; } = 0;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Posts.Posts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; }
    }
}