using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class CommentGetDTO
    {
        public Guid Id { get; set; } = default!;

        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }

        public string CommentValue { get; set; } = default!;
        public DateTime CommentDateTime { get; set; }
    }

    public class CommentCreateDTO
    {
        [Display(Name = nameof(PostId), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid PostId { get; set; } = default!;

        [Display(Name = nameof(CommentValue), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MinLength")]
        public string CommentValue { get; set; } = default!;
    }

    public class CommentEditDTO
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid Id { get; set; } = default!;

        [MaxLength(300)] [MinLength(1)] public string CommentValue { get; set; } = default!;
    }

    public class CommentAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(CommentValue), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MinLength")]
        public string CommentValue { get; set; } = default!;

        [Display(Name = nameof(CommentDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public DateTime CommentDateTime { get; set; } = DateTime.Now;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(PostId), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid PostId { get; set; } = default!;
    }
}