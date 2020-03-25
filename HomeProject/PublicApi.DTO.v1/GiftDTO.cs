using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class GiftDTO: DomainEntity
    {
        [MaxLength(100)] public string GiftName { get; set; } = default!;
        [MaxLength(300)] public string? GiftImageUrl { get; set; }
    }
}