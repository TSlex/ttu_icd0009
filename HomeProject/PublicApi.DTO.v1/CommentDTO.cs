using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class CommentDTO: DomainEntityBaseMetadata
    {
        [MaxLength(300)][MinLength(1)] public string CommentValue { get; set; } = default!;
        public DateTime CommentDateTime { get; set; } = DateTime.Now;

        public Guid ProfileId { get; set; } = default!;

        public Guid PostId { get; set; } = default!;
    }
}