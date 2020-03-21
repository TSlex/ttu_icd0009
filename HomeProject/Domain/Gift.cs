using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Gift: DomainEntity
    {
        [MaxLength(100)] public string GiftName { get; set; } = default!;
        [MaxLength(300)] public string? GiftImageUrl { get; set; }
        
        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }
}