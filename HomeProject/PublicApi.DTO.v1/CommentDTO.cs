using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class CommentGetDTO
    {
        public string UserName { get; set; }
        public string? ProfileAvatarUrl { get; set; }

        public string CommentValue { get; set; }
        public DateTime CommentDateTime { get; set; }
    }

    public class CommentCreateDTO
    {
        public Guid PostId { get; set; } = default!;

        [MaxLength(300)] [MinLength(1)] public string CommentValue { get; set; } = default!;
    }

    public class CommentEditDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid PostId { get; set; } = default!;

        [MaxLength(300)] [MinLength(1)] public string CommentValue { get; set; } = default!;
    }
}