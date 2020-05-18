using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Comment : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(CommentValue), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        [MaxLength(300)]
        [MinLength(1)]
        public string CommentValue { get; set; } = default!;

        [Display(Name = nameof(CommentDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public DateTime CommentDateTime { get; set; } = DateTime.Now;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(PostId), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public Guid PostId { get; set; } = default!;

        [Display(Name = nameof(Post), ResourceType = typeof(Resourses.BLL.App.DTO.Comments.Comments))]
        public Post? Post { get; set; }

        [Display(Name = nameof(ReturnUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? ReturnUrl { get; set; }
    }
}