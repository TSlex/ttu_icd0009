using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Comment: DomainEntityMetadata
    {
        [MaxLength(300)][MinLength(1)] public string CommentValue { get; set; } = default!;
        public DateTime CommentDateTime { get; set; } = DateTime.Now;

        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        [MaxLength(36)] public string PostId { get; set; } = default!;
        public Post? Post { get; set; }
    }
}