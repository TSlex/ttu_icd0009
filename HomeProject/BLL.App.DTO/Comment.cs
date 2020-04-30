using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Comment: DomainEntityBaseMetadata
    {
        [MaxLength(300)][MinLength(1)] public string CommentValue { get; set; } = default!;
        public DateTime CommentDateTime { get; set; } = DateTime.Now;

        public Guid ProfileId { get; set; } = default!;
        public ProfileFull? Profile { get; set; }

        public Guid PostId { get; set; } = default!;
        public Post? Post { get; set; }
        
        //TODO: split
        public string? ReturnUrl { get; set; }
    }
}