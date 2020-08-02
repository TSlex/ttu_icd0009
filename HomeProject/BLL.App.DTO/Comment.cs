using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using DomainEntityBaseMetaSoftUpdateDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftUpdateDelete;

namespace BLL.App.DTO
{
    public class Comment : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(CommentValue), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MinLength")]
        public string CommentValue { get; set; } = default!;

        [Display(Name = nameof(CommentDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public DateTime CommentDateTime { get; set; } = DateTime.Now;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(PostId), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid PostId { get; set; } = default!;

        [Display(Name = nameof(Post), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public Post? Post { get; set; }

        [Display(Name = nameof(ReturnUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? ReturnUrl { get; set; }
    }
}