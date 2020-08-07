using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class Comment: DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(300)][MinLength(1)] public string CommentValue { get; set; } = default!;
        public DateTime CommentDateTime { get; set; } = DateTime.UtcNow;

        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        public Guid PostId { get; set; } = default!;
        public Post? Post { get; set; }
    }
}