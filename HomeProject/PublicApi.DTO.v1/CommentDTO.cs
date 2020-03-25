using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class CommentDTO: DomainEntity
    {
        [MaxLength(300)][MinLength(1)] public string CommentValue { get; set; } = default!;
        public DateTime CommentDateTime { get; set; } = DateTime.Now;

        [MaxLength(36)] public string ProfileId { get; set; } = default!;

        [MaxLength(36)] public string PostId { get; set; } = default!;
    }
}